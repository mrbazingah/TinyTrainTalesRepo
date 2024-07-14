using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cityText;
    [SerializeField] TextMeshProUGUI underText;
    [SerializeField] float speed;

    bool mouseIsOnMenu;

    Color startColor;
    bool isDone;

    CityManager cityManager;
    GameManager gameManager;

    void Awake()
    {
        cityManager = FindObjectOfType<CityManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        underText.color = new Color(255, 255, 255, 0);
        startColor = underText.color;

        isDone = true;
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

    void Update()
    {
        if (!isDone && underText.color.a < 255)
        {
            underText.color = new Color(255, 255, 255, underText.color.a + speed);

            StartCoroutine(ResetUnderText());
        }
        else if (isDone && underText.color.a > 0)
        {
            underText.color = new Color(255, 255, 255, underText.color.a - speed);
        }
    }

    IEnumerator ResetUnderText()
    {
        yield return new WaitForSeconds(3);

        isDone = true;
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

        isDone = false;
    }
}
