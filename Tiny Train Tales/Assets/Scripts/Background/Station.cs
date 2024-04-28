using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour
{
    [SerializeField] GameObject stationCanvas;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] TextMeshProUGUI boardedPassangersText;
    [SerializeField] TextMeshProUGUI deboardedPassangersText;

    bool hasArrived;

    GameManager gameManager;
    CityManager cityManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        cityManager = FindObjectOfType<CityManager>();
    }

    void Start()
    {
        stationCanvas.SetActive(false);
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

    public void GetPassangers(float subPassangers, float addPassangers)
    {
        boardedPassangersText.text = addPassangers.ToString();
        deboardedPassangersText.text = subPassangers.ToString();
    }

    public void LeaveStastion()
    {
        CameraMovement cam = FindObjectOfType<CameraMovement>();
        float camPosX = cam.transform.position.x;
        PlayerPrefs.SetFloat("CamPos", camPosX);

        gameManager.DeleteSavedDestination();
        cityManager.SaveCurrentCity();

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
