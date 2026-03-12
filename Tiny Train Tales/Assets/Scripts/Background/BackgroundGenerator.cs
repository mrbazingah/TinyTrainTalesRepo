using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    Train train;
    Rigidbody2D myRigidbody;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    public GameObject SpawnBlock(float yPos, float spawnOffset, GameObject blockPrefab)
    {
        Vector2 spawnPos = new Vector2(blockPrefab.transform.position.x + spawnOffset, yPos);
        GameObject spawned = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        return spawned;
    }

    public void RemoveBlock(GameObject block)
    {
        if (myRigidbody == train.GetRigidbody())
        {
            train.FindRigidbody();
        }

        Destroy(block);
    }
}