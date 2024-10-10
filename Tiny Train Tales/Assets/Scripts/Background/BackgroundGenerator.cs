using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] float spawnOffset;
    [SerializeField] bool canSpawn;

    Train train;
    GameManager gameManager;

    private const string TRAIN_TAG = "Train";

    void Awake()
    {
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TRAIN_TAG) && canSpawn)
        {
            SpawnBlock();
        }
    }

    public void SpawnBlock()
    {
        if (blockPrefab != null)
        {
            GameObject spawned = Instantiate(blockPrefab, new Vector2(transform.position.x + spawnOffset, transform.position.y), Quaternion.identity);
        }
        else
        {
            Debug.LogError("blockPrefab is not assigned!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TRAIN_TAG))
        {
            train.FindRigidbody();
            Destroy(gameObject, 5);
        }
    }

    public void ChangeSpawnOffset()
    {
        spawnOffset = -spawnOffset;
    }

    public float GetSpawnOffset()
    {
        return spawnOffset;
    }
}