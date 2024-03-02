using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;

    float speed;

    Rigidbody2D myRigidbody;
    Train train;
    GameManager gameManager;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();  
        train = FindObjectOfType<Train>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
       
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
            Destroy(gameObject);
        }
    }
}
