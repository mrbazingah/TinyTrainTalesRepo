using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockMovement : MonoBehaviour
{
    [SerializeField] float speedOffset = 1f;
    [SerializeField] bool canSpawn;
    [SerializeField] int currentBlockNumber;
    [SerializeField] float spawnOffset;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Train" && canSpawn)
        {
            backgroundGenerator.SpawnBlock(transform.position.y, spawnOffset, gameObject);
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
