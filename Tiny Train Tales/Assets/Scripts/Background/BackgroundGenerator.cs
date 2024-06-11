using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] float spawnOffset;
    [SerializeField] bool canSpawn;

    GameObject parent;

    Train train;
    GameManager gameManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        parent = GameObject.Find("BackgroundParent");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Train") && canSpawn)
        {
            SpawnBlock();
        }
    }

    public void SpawnBlock()
    {
        GameObject spawned = Instantiate(blockPrefab, new Vector2(transform.position.x + spawnOffset, transform.position.y), Quaternion.identity);
        if (parent != null)
        {
            spawned.transform.SetParent(parent.transform);
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

    public void ChangeSpawnOffset()
    {
        spawnOffset = -spawnOffset;
    }

    public float GetSpawnOffset()
    {
        return spawnOffset;
    }
}
