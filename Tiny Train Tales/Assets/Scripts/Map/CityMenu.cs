using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cityText;
    [SerializeField] TextMeshProUGUI countrytext;
    [SerializeField] float speed;

    bool mouseIsOver;

    CityManager cityManager;
    GameManager gameManager;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnMouseEnter()
    {
        Region[] regions = FindObjectsOfType<Region>();
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i].GetMouseIsOver()) { return; }
        }

        mouseIsOver = true;
    }

    void OnMouseExit()
    {
        mouseIsOver = false;
    }

    public bool GetMouseIsOver()
    {
        return mouseIsOver;
    }

    public void TravelButton()
    {
        gameManager.SaveAll();

        GameObject newDestinationCity = GameObject.Find(cityText.text);
        if (newDestinationCity == cityManager.GetDestinationCity()) { return; }

        cityManager.SetNewDestinationCity(newDestinationCity);
        PlayerPrefs.SetInt("OpenMap", 1);

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetTexts(string cityName, string countryName)
    {
        cityText.text = cityName;
        countrytext.text = countryName;
    }

    public bool GetMouseIsOnMenu()
    {
        return mouseIsOver;
    }
}
