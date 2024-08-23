using TMPro;
using UnityEngine;

public class DistanceQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] int distanceToTravel;

    bool hasFinishedQuest;

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
        if (PlayerPrefs.HasKey(gameObject.name + "QuestDistanceTraveled") && PlayerPrefs.HasKey(gameObject.name + "QuestDistanceToTravel") && PlayerPrefs.HasKey(gameObject.name + "QuestDifference"))
        {
            savedDistanceTraveled = PlayerPrefs.GetFloat(gameObject.name + "QuestDistanceTraveled");
            distanceToTravel = PlayerPrefs.GetInt(gameObject.name + "QuestDistanceToTravel");
            difference = PlayerPrefs.GetFloat(gameObject.name + "QuestDifference");

            Debug.Log("Loaded Distance Quest");
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
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDistanceTraveled");
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDistanceToTravel");
        PlayerPrefs.DeleteKey(gameObject.name + "QuestDifference");
    }

    public void SaveTravelDistance()
    {
        PlayerPrefs.SetFloat(gameObject.name + "QuestDistanceTraveled", distanceTraveled);
        PlayerPrefs.SetInt(gameObject.name + "QuestDistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat(gameObject.name + "QuestDifference", difference);

        Debug.Log("Saved Distance Quest");
    }
}
