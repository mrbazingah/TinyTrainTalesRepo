using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour
{
    [SerializeField] GameObject stationCanvas;

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

    public void LeaveStastion()
    {
        CameraMovement cam = FindObjectOfType<CameraMovement>();
        float camPosX = cam.transform.position.x;
        PlayerPrefs.SetFloat("CamPos", camPosX);

        gameManager.ResetDestination();

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
