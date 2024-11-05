using UnityEngine;
using System.Collections.Generic;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockParent;
    [SerializeField] float spawnOffset;
    [SerializeField] bool canSpawn;

    List<GameObject> allBlocks = new List<GameObject>(0);

    Train train;
    GameManager gameManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }
    
    public void SpawnBlock(float yPos, GameObject blockPrefab)
    {
        if (blockPrefab != null && canSpawn)
        {
            Vector2 spawnPos = new Vector2(allBlocks[allBlocks.Count - 1].transform.position.x + spawnOffset, yPos);

            GameObject spawned = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
            allBlocks.Add(spawned);

            Debug.Log("Spawned");
        }
    }

    void Update()
    {
        UpdateParent();
    }

    void UpdateParent()
    {
        if (allBlocks.Count > 0) { return; }

        for (int i = 0; i < allBlocks.Count; i++)
        {
            allBlocks[i].transform.SetParent(blockParent.transform);
        }
    }

    public void RemoveBlock(GameObject block)
    {
        allBlocks.Add(block);

        train.FindRigidbody();
        Destroy(block, 5);
    }

    public float GetSpawnOffset()
    {
        return spawnOffset;
    }
}