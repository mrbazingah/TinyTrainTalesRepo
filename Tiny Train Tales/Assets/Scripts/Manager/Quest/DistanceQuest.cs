using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] GameObject collectButton;
    [SerializeField] int distanceToTravel;
    [SerializeField] int distanceOffset;

    bool hasFinishedQuest;

    float distanceTraveled;
    float savedDistanceTraveled;
    float difference;

    GameManager gameManager;
    QuestManager questManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "QuestDistanceTraveled"))
        {
            savedDistanceTraveled = PlayerPrefs.GetFloat(gameObject.name + "QuestDistanceTraveled") + distanceOffset;
            distanceToTravel = PlayerPrefs.GetInt(gameObject.name + "QuestDistanceToTravel");
            difference = PlayerPrefs.GetFloat(gameObject.name + "QuestDifference");
        }
        else
        {
            SetNewQuests();
        }
    }

    void SetNewQuests()
    {
        int temp = (int)Random.Range(1, 10);
        distanceToTravel = temp * 1000;
        questText.text = "Travel " + distanceToTravel.ToString() + " km";
    }

    void Update()
    {
        CountDownDistance();
    }

    void CountDownDistance()
    {
        if (hasFinishedQuest) return; // Prevent unnecessary calculations

        distanceTraveled = gameManager.GetDistance() - gameManager.GetRemainingDistance();
        difference = Mathf.Round(distanceToTravel - savedDistanceTraveled - distanceTraveled);

        if (difference <= 0)
        {
            questText.text = "Complete";
            collectButton.SetActive(true);
            hasFinishedQuest = true; // Mark as finished to prevent further updates
        }
        else
        {
            questText.text = "Travel " + difference.ToString() + " km";
        }

        Debug.Log($"Distance Traveled: {distanceTraveled}, Difference: {difference}");
    }

    void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDistanceTraveled");
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDistanceToTravel");
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDifference");
    }

    public void SaveTravelDistance()
    {
        PlayerPrefs.SetFloat(gameObject.name + "QuestDistanceTraveled", distanceTraveled);
        PlayerPrefs.SetInt(gameObject.name + "QuestDistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat(gameObject.name + "QuestDifference", difference);
    }

    public void Collect()
    {
        // Assuming this method gets called when the quest is collected.
        // Add logic to handle rewards and reset state.
        DeleteKeys();
        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}