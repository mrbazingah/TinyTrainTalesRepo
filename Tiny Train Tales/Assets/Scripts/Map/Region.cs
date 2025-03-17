using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities; 
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] Color selectColor;
    [SerializeField] GameObject unlockButton;

    Image coverImage;   
    Color startColor;

    bool isUnlocked;    
    bool mouseIsOver;
    bool isSelected;

    void Start()
    {
        coverImage = GetComponent<Image>();
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
        if (!mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            coverImage.color = startColor;

            if (isUnlocked)
            {
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }

            unlockButton.SetActive(false);
        }
        else if (mouseIsOver && Input.GetKeyDown(KeyCode.Mouse0))
        {
            coverImage.color = selectColor;
            unlockButton.SetActive(true);
        }
    }

    public void OnButtonClick()
    {
       Debug.Log("Button Clicked");    
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

    public bool GetIsSelected()
    {
        return isSelected;
    }
}
