using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities;
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] GameObject cover;

    bool isUnlocked;

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        for (int i = 0; i < regionCities.Length; i++)
        {
            regionCities[i].SetActive(isUnlocked);

            City cityComp = regionCities[i].GetComponent<City>();
            GameObject[] currentCityLines = cityComp.GetCityNeighborLines();
            for (int ii = 0; ii < currentCityLines.Length; ii++)
            {
                if (!regionCitiesLines.Contains(currentCityLines[ii]))
                {
                    regionCitiesLines.Add(currentCityLines[ii]);
                }
        
                currentCityLines[ii].SetActive(isUnlocked);
            }
        }

        cover.SetActive(!isUnlocked);
    }

    void Update()
    {
        UpdateCityActivity();
    }

    public void UpdateCityActivity()
    {
        for (int i = 0; i < regionCitiesLines.Count; i++)
        {
            if (!regionCitiesLines[i].activeSelf) continue;

            regionCitiesLines[i].SetActive(isUnlocked);
        }
    }


    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }
}
