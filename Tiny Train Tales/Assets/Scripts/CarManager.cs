using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject[] currentCars;
    [SerializeField] float spawnOffset;
    
    Vector2 startPos;

    void Start()
    {
        currentCars = GameObject.FindGameObjectsWithTag("Car");
    }

    public void AddCar()
    {
        int length = currentCars.Length + 1;
        Debug.Log(length.ToString());

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
    }
}
