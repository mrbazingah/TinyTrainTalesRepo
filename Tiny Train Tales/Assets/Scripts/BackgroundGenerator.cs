using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        GameObject spawnedBlock = Instantiate(blockPrefab, new Vector2(transform.position.x + 20, transform.position.y), Quaternion.identity);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Train"))
        {
            Destroy(gameObject);
        }
    }
}
