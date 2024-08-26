using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

        collectButton.SetActive(false);
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

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    void CountDownDistance()
    {
        if (difference <= 0)
        {
            questText.text = "Complete";
            collectButton.SetActive(true);
        }
        else
        {
            distanceTraveled = gameManager.GetDistance() - gameManager.GetRemainingDistance();
            difference = Mathf.Round(distanceToTravel - savedDistanceTraveled - distanceTraveled);
            questText.text = "Travel " + difference.ToString() + " km";
        }
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
        gameManager.AddToGems(distanceToTravel / 1000);
        DeleteKeys();

        List<GameObject> instantiatedQuests = questManager.GetInstantiatedQuests();
        int number = 0;

        for (int i = 0; i < instantiatedQuests.Count; i++)
        {
            if (gameObject == instantiatedQuests[i])
            {
                number = i;
                break;
            }
        }

        questManager.RemoveInstantiatedQuest(number);
        questManager.ResetQuests();

        Destroy(gameObject);
    }
}
