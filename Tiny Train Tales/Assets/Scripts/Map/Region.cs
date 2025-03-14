using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Region : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities; // For global access.
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] GameObject cover;
    [SerializeField] Color selectColor;

    Image coverImage;
    Color startColor;

    bool isUnlocked;
    bool mouseOver;

    void Start()
    {
        if (cover == null)
        {
            Debug.LogError("Cover is not assigned in " + gameObject.name);
            return;
        }
        coverImage = cover.GetComponent<Image>();
        if (coverImage == null)
        {
            Debug.LogError("Cover does not have an Image component on " + cover.name);
        }
        else
        {
            startColor = coverImage.color;
        }
    }

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        // Activate or deactivate each city and collect their connection lines.
        foreach (GameObject city in regionCities)
        {
            if (city == null)
                continue;

            city.SetActive(isUnlocked);
            City cityComp = city.GetComponent<City>();
            if (cityComp == null)
            {
                Debug.LogWarning("City component missing on " + city.name);
                continue;
            }

            GameObject[] cityLines = cityComp.GetCityNeighborLines();
            foreach (GameObject line in cityLines)
            {
                if (line == null)
                    continue;
                if (!regionCitiesLines.Contains(line))
                {
                    regionCitiesLines.Add(line);
                }
                line.SetActive(isUnlocked);
            }
        }

        // When the region is locked (not unlocked), ensure the cover is active.
        if (cover != null)
        {
            cover.SetActive(!isUnlocked);
        }
    }

    // IPointerClickHandler: called when this object is clicked.
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Region " + gameObject.name + " was clicked.");
        if (coverImage != null)
        {
            coverImage.color = selectColor;
            Debug.Log("Cover color set to " + selectColor);
        }
        // If locked, keep the cover on. If unlocked, toggle its active state.
        if (!isUnlocked)
        {
            cover.SetActive(true);
            Debug.Log("Region is locked, so cover remains active.");
        }
        else
        {
            bool newState = !cover.activeSelf;
            cover.SetActive(newState);
            Debug.Log("Region is unlocked, toggling cover to " + newState);
        }
    }

    // IPointerEnterHandler: fires when pointer enters the area.
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        Debug.Log("Pointer entered " + gameObject.name);
    }

    // IPointerExitHandler: fires when pointer exits the area.
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        Debug.Log("Pointer exited " + gameObject.name);
    }

    // Returns the region's neighbors.
    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }

    public GameObject[] GetRegionCities()
    {
        return regionCities;
    }
}
