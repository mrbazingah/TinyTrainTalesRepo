using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cityText;

    bool mouseIsOnMenu;

    CityManager cityManager;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
    }

    void OnMouseEnter()
    {
        mouseIsOnMenu = true;
    }

    void OnMouseExit()
    {
        mouseIsOnMenu = false;
    }

    public bool GetMouseIsOnMenu()
    {
        return mouseIsOnMenu;
    }

    public void TravelButton()
    {
        GameObject newDestinationCity = GameObject.Find(cityText.text);
        cityManager.SetNewDestinationCity(newDestinationCity);

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
