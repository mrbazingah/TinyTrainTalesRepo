using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField] List<CityNamespace.City> allCities;
    [SerializeField] DistanceData distanceData;

    void Start()
    {
        AssignDistances();
    }

    void AssignDistances()
    {
        foreach (CityNamespace.City cityA in allCities)
        {
            foreach (CityNamespace.City cityB in allCities)
            {
                if (cityA != cityB && !cityA.neighbors.ContainsKey(cityB))
                {
                    float distance = distanceData.GetDistance(cityA, cityB);
                    cityA.neighbors.Add(cityB, distance);
                    cityB.neighbors.Add(cityA, distance);
                }
            }
        }
    }
}
namespace CityNamespace
{
    public class City
    {
        public Vector2 position;
        public Dictionary<City, float> neighbors;
        public float gCost;
        public float hCost;
        public City parent;

        public City(Vector2 _position)
        {
            position = _position;
            neighbors = new Dictionary<City, float>();
            gCost = Mathf.Infinity;
            hCost = 0;
            parent = null;
        }

        public float fCost
        {
            get { return gCost + hCost; }
        }
    }
}

[System.Serializable]
public class DistanceData
{
    public List<DistanceEntry> distances;

    public float GetDistance(CityNamespace.City cityA, CityNamespace.City cityB)
    {
        foreach (DistanceEntry entry in distances)
        {
            if ((entry.cityA == cityA && entry.cityB == cityB) || (entry.cityA == cityB && entry.cityB == cityA))
            {
                return entry.distance;
            }
        }
        return float.MaxValue; // Return a large value if distance is not found
    }
}

[System.Serializable]
public class DistanceEntry
{
    public CityNamespace.City cityA;
    public CityNamespace.City cityB;
    public float distance;
}
