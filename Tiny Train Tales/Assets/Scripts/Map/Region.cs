using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities; 
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] Color selectColor; 

    Image coverImage;   
    Color startColor;   

    bool isUnlocked;    
    bool mouseIsOver;   

    void Start()
    {
        // Get the Image component from the same GameObject (this is your cover)
        coverImage = GetComponent<Image>();
        if (coverImage == null)
        {
            Debug.LogError("No Image component found on " + gameObject.name);
            return;
        }
        // Save the original color (which should have a nonzero alpha)
        startColor = coverImage.color;
    }

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

        // Update each city in the region and collect connection lines.
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
                // Set the connection line's active state based on region state.
                currentCityLines[ii].SetActive(isUnlocked);
            }
        }

        // Instead of turning off the cover, adjust its transparency.
        if (coverImage != null)
        {
            if (isUnlocked)
            {
                // Unlocked: make cover invisible by setting alpha to 0.
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }
            else
            {
                // Locked: show the cover with its original color.
                coverImage.color = startColor;
            }
        }
    }

    void Update()
    {
        // When the mouse is over this region and clicked, change the cover color to selectColor.
        if (mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            coverImage.color = selectColor;
        }
        // When clicking outside this region (mouse not over), reset to startColor.
        else if (!mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Reset the color to the starting color.
            coverImage.color = startColor;
            // If the region is unlocked, make it invisible (alpha 0).
            if (isUnlocked)
            {
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }
            // Otherwise (if locked), keep the original visible color.
        }
    }

    void OnMouseEnter()
    {
        mouseIsOver = true;
    }

    void OnMouseExit()
    {
        mouseIsOver = false;
    }

    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }

    public GameObject[] GetRegionCities()
    {
        return regionCities;
    }
}
