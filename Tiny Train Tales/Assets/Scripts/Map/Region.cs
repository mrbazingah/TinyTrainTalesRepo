using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Region : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] public GameObject[] regionCities; // For global access.
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] GameObject cover;
    [SerializeField] Color selectColor;

    Image coverImage;
    Color startColor;

    bool isUnlocked;
    bool mouseOver;

    void Start()
    {
        coverImage = cover.GetComponent<Image>();
        startColor = coverImage.color;
    }

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        // Set each city's active state and gather its connection lines.
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
                currentCityLines[ii].SetActive(isUnlocked);
            }
        }

        // The cover is active when the region is locked.
        if (cover != null)
        {
            cover.SetActive(!isUnlocked);
        }
    }

    // IPointerClickHandler implementation.
    public void OnPointerClick(PointerEventData eventData)
    {
        // Change the cover color on click.
        coverImage.color = selectColor;
        // If the region is locked, always keep the cover active.
        // Otherwise, toggle its active state.
        if (!isUnlocked)
        {
            cover.SetActive(true);
        }
        else
        {
            cover.SetActive(!cover.activeSelf);
        }
    }

    // IPointerEnterHandler implementation.
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    // IPointerExitHandler implementation.
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    // Returns the region's neighbor GameObjects.
    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }
}
