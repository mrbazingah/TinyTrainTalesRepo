using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour
{
    [SerializeField] GameObject stationCanvas;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] TextMeshProUGUI boardedPassangersText;
    [SerializeField] TextMeshProUGUI deboardedPassangersText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Canvas canvas;

    bool hasArrived;

    GameManager gameManager;
    CityManager cityManager;
    CityMarketMenu cityCargoMenu;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cityManager = FindObjectOfType<CityManager>();
        cityCargoMenu = FindObjectOfType<CityMarketMenu>();
    }

    void Start()
    {
        stationCanvas.SetActive(false);
        SetUpCanvas();
    }

    void SetUpCanvas()
    {
        canvas.worldCamera = Camera.main;
    }

    void Update()
    {
        hasArrived = gameManager.GetHasArrivedAtStation();
        if (hasArrived)
        {
            stationCanvas.SetActive(true);
        }

        if (gameObject.name == "Station Block(Clone)(Clone)")
        {
            Instantiate(blockPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void GetPassangers(float subPassangers, float addPassangers, float coinsAdded)
    {
        boardedPassangersText.text = addPassangers.ToString();
        deboardedPassangersText.text = subPassangers.ToString();
        coinsText.text = "+" + coinsAdded.ToString();
    }

    public void OpenCargoMenu()
    {
        cityCargoMenu.OpenCargoMenu();
    }

    public void LeaveStation()
    {
        CameraMovement cam = FindObjectOfType<CameraMovement>();
        float camPosX = cam.transform.position.x;
        PlayerPrefs.SetFloat("CamPos", camPosX);

        gameManager?.DeleteSavedDestination(false);
        cityManager?.SaveOnDeparture();
        cityManager?.ResetPath();

        PlayerPrefs.DeleteKey("Dont Destroy");

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
