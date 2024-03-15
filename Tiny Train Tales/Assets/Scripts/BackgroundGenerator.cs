using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject stationBlockPrefab;
    [SerializeField] float spawnOffset;
    [SerializeField] bool canSpawn;

    Train train;
    GameManager gameManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Train") && canSpawn)
        {
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        bool arrivedAtStation = gameManager.GetArrivedAtStation();
        if (gameObject.tag == "Block" && arrivedAtStation)
        {
            Instantiate(stationBlockPrefab, new Vector2(transform.position.x + spawnOffset, transform.position.y), Quaternion.identity);
        }
        else
        {
            Instantiate(blockPrefab, new Vector2(transform.position.x + spawnOffset, transform.position.y), Quaternion.identity);
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            Destroy(gameObject);
            gameObject.SetActive(false);
            train.FindRigidbody();
        }
    }
}
