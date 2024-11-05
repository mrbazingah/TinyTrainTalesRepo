using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] float speedOffset = 1f;

    float speed;

    Rigidbody2D myRigidbody;
    Train train;
    BackgroundGenerator backgroundGenerator;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        train = FindObjectOfType<Train>();
        backgroundGenerator = FindObjectOfType<BackgroundGenerator>();
    }

    void Start()
    {
        LoadSpeed();
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

    void FixedUpdate()
    {
        speed = train.GetSpeed();
        myRigidbody.velocity = new Vector2(-speed * speedOffset, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            backgroundGenerator.SpawnBlock(transform.position.y, blockPrefab);
            Debug.Log("triggered");
        }

        Debug.Log("triggered but not train");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            backgroundGenerator.RemoveBlock(gameObject);
        }
    }
}