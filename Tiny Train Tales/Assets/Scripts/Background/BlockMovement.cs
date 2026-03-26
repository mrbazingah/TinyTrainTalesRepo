using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Sky Block Renderers (Block 4 only)")]
    [SerializeField] SpriteRenderer[] morningRenderers;
    [SerializeField] SpriteRenderer[] dayRenderers;
    [SerializeField] SpriteRenderer[] eveningRenderers;
    [SerializeField] SpriteRenderer[] nightRenderers;

    [Header("Non-Sky Block Tinting")]
    [SerializeField] List<SpriteRenderer> spriteRenderers;
    [SerializeField] bool includeTrain;
    [SerializeField] ParticleSystem smokePS;

    ParticleSystem.MainModule psMainModule;
    float speed;
    List<ColorBlock> originalColors = new List<ColorBlock>();
    Color psOriginalColor;
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
            originalColors = new List<ColorBlock>();
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                ColorBlock cb = ColorBlock.defaultColorBlock;
                if (spriteRenderers[i] != null)
                    cb.normalColor = spriteRenderers[i].color;
                originalColors.Add(cb);
            }
        }

        psMainModule = smokePS.main;
        psOriginalColor = psMainModule.startColor.color;

        LoadSpeed();
        DayNightEvent();
    }

    public void SetOriginalColor(List<ColorBlock> spawnParentOriginalColors)
    {
        originalColorsInitialized = true;
        originalColors = spawnParentOriginalColors;
        DayNightCycle.TimeOfDay timeOfDay = dayNightCycle.currentTime;
        lastTimeOfDay = timeOfDay;

        if (spriteRenderers.Count > 0)
        {
            Color blockColor = dayNightCycle.GetBlendedColor();
            for (int i = 0; i < spriteRenderers.Count; i++)
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
        SetTrainSpriteRenderers();
    }

    void DayNightEvent()
    {
        bool transitioning = dayNightCycle.IsTransitioning();
        DayNightCycle.TimeOfDay timeOfDay = dayNightCycle.currentTime;

        if (!transitioning && timeOfDay == lastTimeOfDay) return;

        Color blockColor = transitioning ? dayNightCycle.GetBlendedColor() : dayNightCycle.GetTimeColor(timeOfDay);

        if (currentBlockNumber == 3)
        {
            HandleSkyBlock(transitioning, timeOfDay);
        }
        else if (spriteRenderers.Count > 0)
        {
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                if (spriteRenderers[i] != null)
                    spriteRenderers[i].color = originalColors[i].normalColor * blockColor;
            }
        }

        if (includeTrain)
        {
            psMainModule.startColor = new ParticleSystem.MinMaxGradient(psOriginalColor * blockColor);
            ApplyColorToExistingParticles(blockColor);
        }

        if (!transitioning) lastTimeOfDay = timeOfDay;
    }

    void HandleSkyBlock(bool transitioning, DayNightCycle.TimeOfDay timeOfDay)
    {
        if (transitioning)
        {
            GetSkyObject(dayNightCycle.GetNextTime())?.SetActive(true);

            float alpha = 1f - dayNightCycle.GetTransitionProgress();
            SpriteRenderer[] currentSRs = GetSkyRenderers(timeOfDay);
            for (int i = 0; i < currentSRs.Length; i++)
            {
                if (currentSRs[i] != null)
                {
                    Color c = currentSRs[i].color;
                    c.a = alpha;
                    currentSRs[i].color = c;
                }
            }
        }
        else
        {
            // Transition completed or initial setup: deactivate old, activate new, reset alpha
            if ((int)lastTimeOfDay >= 0)
            {
                SpriteRenderer[] oldSRs = GetSkyRenderers(lastTimeOfDay);
                for (int i = 0; i < oldSRs.Length; i++)
                {
                    if (oldSRs[i] != null)
                    {
                        Color c = oldSRs[i].color;
                        c.a = 1f;
                        oldSRs[i].color = c;
                    }
                }
                if (lastTimeOfDay == DayNightCycle.TimeOfDay.Night)
                {
                    for (int i = 0; i < nightRenderers.Length; i++)
                        if (nightRenderers[i] != null) nightRenderers[i].sortingOrder = i;
                }
                GetSkyObject(lastTimeOfDay)?.SetActive(false);
            }
            if (timeOfDay == DayNightCycle.TimeOfDay.Night)
            {
                for (int i = 0; i < nightRenderers.Length; i++)
                    if (nightRenderers[i] != null) nightRenderers[i].sortingOrder = 12 + i;
            }
            GetSkyObject(timeOfDay)?.SetActive(true);
        }
    }

    SpriteRenderer[] GetSkyRenderers(DayNightCycle.TimeOfDay time) => time switch
    {
        DayNightCycle.TimeOfDay.Morning => morningRenderers,
        DayNightCycle.TimeOfDay.Day     => dayRenderers,
        DayNightCycle.TimeOfDay.Evening => eveningRenderers,
        DayNightCycle.TimeOfDay.Night   => nightRenderers,
        _                               => dayRenderers,
    };

    GameObject GetSkyObject(DayNightCycle.TimeOfDay time) => time switch
    {
        DayNightCycle.TimeOfDay.Morning => morningObject,
        DayNightCycle.TimeOfDay.Day     => dayObject,
        DayNightCycle.TimeOfDay.Evening => eveningObject,
        DayNightCycle.TimeOfDay.Night   => nightObject,
        _                               => dayObject,
    };

    void ApplyColorToExistingParticles(Color blockColor)
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[smokePS.particleCount];
        int count = smokePS.GetParticles(particles);

        for (int i = 0; i < count; i++)
            particles[i].startColor = psOriginalColor * blockColor;

        smokePS.SetParticles(particles, count);
    }

    void SetTrainSpriteRenderers()
    {
        if (!includeTrain) return;

        List<SpriteRenderer> trainSRs = train.GetSpriteRenderers();
        for (int i = 0; i < trainSRs.Count; i++)
        {
            if (trainSRs[i] == null || spriteRenderers.Contains(trainSRs[i])) continue;

            spriteRenderers.Add(trainSRs[i]);
            ColorBlock cb = ColorBlock.defaultColorBlock;
            cb.normalColor = trainSRs[i].color;
            originalColors.Add(cb);

            Color blockColor = dayNightCycle.GetBlendedColor();
            trainSRs[i].color = cb.normalColor * blockColor;
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
            //GameObject spawned = backgroundGenerator.SpawnBlock(transform.position.y, spawnOffset, gameObject);
            //spawned.GetComponent<BlockMovement>().SetOriginalColor(originalColors);
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
