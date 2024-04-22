using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour
{
    [SerializeField] GameObject stationCanvas;
    [SerializeField] TextMeshProUGUI boardedPassangersText;
    [SerializeField] TextMeshProUGUI deboardedPassangersText;

    bool hasArrived;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
