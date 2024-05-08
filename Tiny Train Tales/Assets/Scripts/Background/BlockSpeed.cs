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

    }

    void Start()
    {
        LoadSpeed();
    }

    void LoadSpeed()
    {
        if (!train.GetHasLoaded())
        {
            TrainData trainData = SaveSystem.LoadTrain();
            speed = trainData.speed;
            myRigidbody.velocity = new Vector2(-speed * speedOffset, 0f);
            train.LoadTrain();
        }
    }

    void FixedUpdate()
    {
        speed = train.GetSpeed();
        myRigidbody.velocity = new Vector2(-speed * Time.fixedDeltaTime * speedOffset, 0f);
    }
}
