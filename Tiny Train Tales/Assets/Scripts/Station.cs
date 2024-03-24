using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour
{
    [SerializeField] GameObject stationCanvas;

    bool hasArrived;

    Train train;
    GameManager gameManager;

    void Awake()
    {
        train = FindObjectOfType<Train>();
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
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
