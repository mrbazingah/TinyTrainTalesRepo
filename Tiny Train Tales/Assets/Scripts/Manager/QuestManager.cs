using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] int amountOfQuest;

    bool hasFinishedQuest;

    int distanceToTravel;
    float distanceTraveled;
    float savedDistanceTraveled;
    float difference;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();    
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("QuestDistanceTraveled") && PlayerPrefs.HasKey("QuestDistanceToTravel") && PlayerPrefs.HasKey("QuestDifference"))
        {
            savedDistanceTraveled = PlayerPrefs.GetFloat("QuestDistanceTraveled");
            distanceToTravel = PlayerPrefs.GetInt("QuestDistanceToTravel");
            difference = PlayerPrefs.GetFloat("QuestDifference");
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
        distanceTraveled = gameManager.GetDistance() - gameManager.GetRemainingDistance();
        difference = Mathf.Round(distanceToTravel - savedDistanceTraveled - distanceTraveled);
        questText.text = "Travel " + difference.ToString() + " km";

        if (difference <= 0 && !hasFinishedQuest)
        {
            DeleteKeys();
            questText.text = "Mission finished";
            gameManager.AddCoins(distanceToTravel);
            gameManager.AddToGems(5);

            hasFinishedQuest = true;
        }
    }

    void DeleteKeys()
    {
        PlayerPrefs.DeleteKey("QuestDistanceTraveled");
        PlayerPrefs.DeleteKey("QuestDistanceToTravel");
        PlayerPrefs.DeleteKey("QuestDifference");
    }

    public void SaveTravelDistance()
    {
        PlayerPrefs.SetFloat("QuestDistanceTraveled", distanceTraveled);
        PlayerPrefs.SetInt("QuestDistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat("QuestDifference", difference);
    }
}
