using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockParent;

    Train train;
    GameManager gameManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SpawnBlock(float yPos, float spawnOffset, GameObject blockPrefab)
    {
        Vector2 spawnPos = new Vector2(blockPrefab.transform.position.x + spawnOffset, yPos);
        GameObject spawned = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
    }

    public void RemoveBlock(GameObject block)
    {
        train.FindRigidbody();
        Destroy(block, 5);
    }
}