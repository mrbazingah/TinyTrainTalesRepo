using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject[] currentCars;
    [SerializeField] float spawnOffset;
    [Space]
    [SerializeField] BoxCollider2D trainCollider;
    [SerializeField] float colliderOffset;
    [SerializeField] Vector2 startPos;

    int length;

    void Start()
    {
        currentCars = GameObject.FindGameObjectsWithTag("Car");

        if (PlayerPrefs.HasKey("Cars"))
        {
            length = PlayerPrefs.GetInt("Cars");

            AddCar(true);
        }
    }

    public void AddCar(bool isStart)
    {
        if (!isStart)
        {
            length = currentCars.Length + 1;
        }
      

        for (int i = 0; i < currentCars.Length; i++)
        {
            Destroy(currentCars[i]);
        }

        currentCars = new GameObject[length];
        GameObject lastSpawned = null;

        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                lastSpawned = Instantiate(carPrefab, startPos, Quaternion.identity);
            }
            else
            {
                Vector2 spawnPos = new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y);
                GameObject currentlySpanwed = Instantiate(carPrefab, spawnPos, Quaternion.identity);
                lastSpawned = currentlySpanwed;
            }

            currentCars[i] = lastSpawned;
        }

        trainCollider.size = new Vector2(length * colliderOffset, trainCollider.size.y);
    }

    public void RemoveCar()
    {
        length = currentCars.Length - 1;

        for (int i = 0; i < currentCars.Length; i++)
        {
            if (i == 0)
            {
                startPos = currentCars[i].transform.position;
            }

            Destroy(currentCars[i]);
        }

        currentCars = new GameObject[length];
        GameObject lastSpawned = null;

        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                lastSpawned = Instantiate(carPrefab, startPos, Quaternion.identity);
            }
            else
            {
                Vector2 spawnPos = new Vector2(lastSpawned.transform.position.x - spawnOffset, lastSpawned.transform.position.y);
                GameObject currentlySpanwed = Instantiate(carPrefab, spawnPos, Quaternion.identity);
                lastSpawned = currentlySpanwed;
            }

            currentCars[i] = lastSpawned;
        }

        trainCollider.size = new Vector2(length * colliderOffset, trainCollider.size.y);
    }

    public void SaveCars()
    {
        PlayerPrefs.SetInt("Cars", currentCars.Length);
    }
}
