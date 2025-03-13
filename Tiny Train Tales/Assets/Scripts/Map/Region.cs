using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] public GameObject[] regionCities; // Made public for global access.
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] GameObject cover;

    bool isUnlocked;

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        // Set each city’s active state and gather its connection lines.
        for (int i = 0; i < regionCities.Length; i++)
        {
            if (regionCities[i] == null)
                continue;

            regionCities[i].SetActive(isUnlocked);

            City cityComp = regionCities[i].GetComponent<City>();
            if (cityComp == null)
            {
                Debug.LogWarning("City component missing on " + regionCities[i].name);
                continue;
            }

            GameObject[] currentCityLines = cityComp.GetCityNeighborLines();
            for (int ii = 0; ii < currentCityLines.Length; ii++)
            {
                if (currentCityLines[ii] == null)
                    continue;

                if (!regionCitiesLines.Contains(currentCityLines[ii]))
                {
                    regionCitiesLines.Add(currentCityLines[ii]);
                }
                // Set local active state (will be overridden globally if needed).
                currentCityLines[ii].SetActive(isUnlocked);
            }
        }

        // The cover is active when the region is locked.
        if (cover != null)
        {
            cover.SetActive(!isUnlocked);
        }
    }

    // Removed the Update() call to prevent per-frame interference.
    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }
}
