using UnityEngine;
using UnityEngine.UI;
using static DayNightCycle;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] float speedOffset = 1f;
    [SerializeField] bool canSpawn;
    [SerializeField] int currentBlockNumber;
    [SerializeField] float spawnOffset;

    [Header("Sky Block Objects (Block 4 only)")]
    [SerializeField] GameObject morningObject;
    [SerializeField] GameObject dayObject;
    [SerializeField] GameObject eveningObject;
    [SerializeField] GameObject nightObject;

    [Header("Non-Sky Block Tinting")]
    [SerializeField] SpriteRenderer[] spriteRenderers;

    float speed;
    ColorBlock[] originalColors;
    bool originalColorsInitialized;
    DayNightCycle.TimeOfDay lastTimeOfDay = (DayNightCycle.TimeOfDay)(-1);

    Rigidbody2D myRigidbody;
    Train train;
    BackgroundGenerator backgroundGenerator;
    DayNightCycle dayNightCycle;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        train = FindObjectOfType<Train>();
        backgroundGenerator = FindObjectOfType<BackgroundGenerator>();
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    void Start()
    {
        if (!originalColorsInitialized)
        {
            originalColorsInitialized = true;
            originalColors = new ColorBlock[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    ColorBlock cb = ColorBlock.defaultColorBlock;
                    cb.normalColor = spriteRenderers[i].color;
                    originalColors[i] = cb;
                }
            }
        }

        LoadSpeed();
        DayNightEvent();
    }

    public void SetOriginalColor(ColorBlock[] spawnParentOriginalColors)
    {
        originalColorsInitialized = true;
        originalColors = spawnParentOriginalColors;
        DayNightCycle.TimeOfDay timeOfDay = dayNightCycle.currentTime;
        lastTimeOfDay = timeOfDay;

        if (spriteRenderers.Length > 0)
        {
            Color blockColor = dayNightCycle.GetTimeColor(timeOfDay);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    spriteRenderers[i].color = originalColors[i].normalColor;
                    spriteRenderers[i].color *= blockColor;
                }
            }
        }
    }

    void LoadSpeed()
    {
        if (!train.GetHasLoaded())
        {
            train.LoadTrain();
            speed = PlayerPrefs.GetFloat("Speed");
            myRigidbody.velocity = new Vector2(-speed * speedOffset, 0f);
        }
    }

    void Update()
    {
        DayNightEvent();
    }

    void DayNightEvent()
    {
        DayNightCycle.TimeOfDay timeOfDay = dayNightCycle.currentTime;
        if (timeOfDay == lastTimeOfDay) return;
        lastTimeOfDay = timeOfDay;

        if (currentBlockNumber == 3)
        {
            if (morningObject != null) morningObject.SetActive(timeOfDay == DayNightCycle.TimeOfDay.Morning);
            if (dayObject != null)     dayObject.SetActive(timeOfDay == DayNightCycle.TimeOfDay.Day);
            if (eveningObject != null) eveningObject.SetActive(timeOfDay == DayNightCycle.TimeOfDay.Evening);
            if (nightObject != null)   nightObject.SetActive(timeOfDay == DayNightCycle.TimeOfDay.Night);
        }
        else if (spriteRenderers.Length > 0)
        {
            Color blockColor = dayNightCycle.GetTimeColor(timeOfDay);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    spriteRenderers[i].color = originalColors[i].normalColor;
                    spriteRenderers[i].color *= blockColor;
                }
            }
        }
    }

    void FixedUpdate()
    {
        speed = train.GetSpeed();
        myRigidbody.velocity = new Vector2(-speed * speedOffset, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Train" && canSpawn)
        {
            GameObject spawned = backgroundGenerator.SpawnBlock(transform.position.y, spawnOffset, gameObject);
            spawned.GetComponent<BlockMovement>().SetOriginalColor(originalColors);

            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            backgroundGenerator.RemoveBlock(gameObject);
        }
    }
}