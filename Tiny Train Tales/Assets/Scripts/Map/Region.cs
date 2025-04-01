using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    [SerializeField] GameObject[] neighbors;
    [SerializeField] GameObject[] regionCities; 
    [SerializeField] List<GameObject> regionCitiesLines;
    [SerializeField] Color selectColor;
    [SerializeField] GameObject unlockButton;
    [SerializeField] int regionNumber;
    [SerializeField] string startCity;
    [SerializeField] string destinationCity;

    Image coverImage;   
    Color startColor;

    bool isUnlocked;    
    bool mouseIsOver;
    bool isSelected;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        coverImage = GetComponent<Image>();
        startColor = coverImage.color;
    }

    public void SetCityActivity(bool active)
    {
        isUnlocked = active;

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

        if (coverImage != null)
        {
            if (isUnlocked)
            {
                coverImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            }
            else
            {
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

            if (!isUnlocked)
            {
                unlockButton.SetActive(true);
            }
        }
    }

    public void UnlockButton()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            PlayerPrefs.SetInt("UnlockedRegion" + regionNumber.ToString(), 1);
            PlayerPrefs.SetString("CurrentCity", startCity);
            PlayerPrefs.SetString("DestinationCity", destinationCity);
            PlayerPrefs.SetString("NextCity", destinationCity);

            SceneManager.LoadScene("GameScene");
        }
        else
        {
            gameManager.UnlockNewRegion(regionNumber);
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

    public bool GetIsSelected()
    {
        return isSelected;
    }
}
