using UnityEngine;

public class BlockSpeed : MonoBehaviour
{
    [SerializeField] float speedOffset = 1f;

    float speed;

    Rigidbody2D myRigidbody;
    Train train;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        train = FindObjectOfType<Train>();

        if (myRigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }

        if (train == null)
        {
            Debug.LogError("Train object not found in the scene!");
        }
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
}