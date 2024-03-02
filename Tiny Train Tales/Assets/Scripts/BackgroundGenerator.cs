using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    Rigidbody2D myRigidbody;
    Train train;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        train = FindObjectOfType<Train>();
    }

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
            train.FindRigidbodies();
            Destroy(gameObject);
        }
    }

    public Rigidbody2D GetRigidbody()
    {
        return myRigidbody;
    }
}
