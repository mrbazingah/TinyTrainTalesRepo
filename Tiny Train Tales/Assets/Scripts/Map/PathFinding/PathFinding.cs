using System.Collections.Generic;
using CityNamespace;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] City startCity;
    [SerializeField] City targetCity;

    void Start()
    {
        FindPath(startCity, targetCity);
    }

    void FindPath(City startCity, City targetCity)
    {
        List<City> openSet = new List<City>();
        HashSet<City> closedSet = new HashSet<City>();
        openSet.Add(startCity);

        while (openSet.Count > 0)
        {
            City currentCity = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentCity.fCost || (openSet[i].fCost == currentCity.fCost && openSet[i].hCost < currentCity.hCost))
                {
                    currentCity = openSet[i];
                }
            }

            openSet.Remove(currentCity);
            closedSet.Add(currentCity);

            if (currentCity == targetCity)
            {
                // Path found
                Debug.Log("Path found!");
                return;
            }

            foreach (KeyValuePair<City, float> neighborPair in currentCity.neighbors)
            {
                City neighbor = neighborPair.Key;
                float distanceToNeighbor = neighborPair.Value;

                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentCity.gCost + distanceToNeighbor;
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetCity);
                    neighbor.parent = currentCity;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        Debug.Log("Path not found!");
    }

    float GetDistance(City cityA, City cityB)
    {
        // You need to define how distance is calculated between cities
        // This could be based on geographical coordinates or any other measure
        // For example, you could calculate the Euclidean distance between their positions
        return Vector2.Distance(cityA.position, cityB.position);
    }
}