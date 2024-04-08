using TMPro;
using UnityEngine;

public class SelectNewCity : MonoBehaviour
{
    [SerializeField] string selectedCity;
    [SerializeField] float distance;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnSelect()
    {
        gameManager.newDestination(selectedCity, distance);
    }
}
