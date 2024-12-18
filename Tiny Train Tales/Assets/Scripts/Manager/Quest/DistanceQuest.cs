using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI travelText;
    [SerializeField] Slider progressSlider;
    [SerializeField] GameObject collectButton;
    [SerializeField] float distanceToTravel;        // Total distance needed to complete the quest
    [SerializeField] float distanceTraveled;        // Distance already traveled
    [SerializeField] float maxDistance, minDistance;
    [SerializeField] float multiplier;
    [SerializeField] float currentDistanceToTravel; // Remaining distance to travel
    [SerializeField] bool getGems; // Specify if this quest gives gems as a reward

    float originalDistance; // Corrected spelling
    bool hasCompleted;

    GameManager gameManager;
    QuestManager questManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();
    }

    void Start()
    {
        collectButton.SetActive(false);
    }

    public void SetUpQuest()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "DistanceToTravel"))
        {
            // Load saved quest data
            distanceToTravel = PlayerPrefs.GetFloat(gameObject.name + "DistanceToTravel");
            distanceTraveled = PlayerPrefs.GetFloat(gameObject.name + "DistanceTraveled");
            originalDistance = PlayerPrefs.GetFloat(gameObject.name + "OriginalDistance", gameManager.GetRemainingDistance());
        }
        else
        {
            // Generate a new quest
            distanceToTravel = Random.Range(minDistance, maxDistance + 1) * multiplier;
            distanceTraveled = 0;
            originalDistance = gameManager.GetRemainingDistance(); // Start tracking from here
        }

        progressSlider.maxValue = distanceToTravel;
        progressSlider.value = distanceTraveled;
        UpdateTravelText();
    }

    void Update()
    {
        TrackTravel();
    }

    void TrackTravel()
    {
        // Ensure original distance is initialized
        if (originalDistance == 0)
        {
            originalDistance = gameManager.GetRemainingDistance();
        }

        // Calculate total distance covered since the quest started
        float currentRemainingDistance = gameManager.GetRemainingDistance();
        float totalCovered = Mathf.Abs(originalDistance - currentRemainingDistance);
        Debug.Log(distanceTraveled.ToString());
        Debug.Log(totalCovered.ToString());

        // Update distanceTraveled only if totalCovered is greater than the saved value
        if (totalCovered > distanceTraveled)
        {
            float progress = totalCovered - distanceTraveled;
            distanceTraveled += progress;
            distanceTraveled = Mathf.Min(distanceTraveled, distanceToTravel); // Cap at total distanceToTravel
            progressSlider.value = distanceTraveled;
        }

        // Calculate the remaining distance
        currentDistanceToTravel = distanceToTravel - distanceTraveled;

        // Prevent negative values
        if (currentDistanceToTravel <= 0 && !hasCompleted)
        {
            currentDistanceToTravel = 0;
            CompleteQuest();
        }

        UpdateTravelText();
    }


    void UpdateTravelText()
    {
        travelText.text = $"Travel: {Mathf.Ceil(currentDistanceToTravel)} km";
    }

    void CompleteQuest()
    {
        if (hasCompleted) return;

        collectButton.SetActive(true);
        hasCompleted = true;
    }

    public void CollectReward()
    {
        float reward;
        if (getGems)
        {
            reward = distanceToTravel / 100; // Example reward calculation for gems
            gameManager.AddGems(reward);
        }
        else
        {
            reward = distanceToTravel * 10; // Example reward calculation for coins
            gameManager.AddCoins(reward);
        }

        ResetQuest();
    }

    void ResetQuest()
    {
        // Clear saved progress and destroy this quest object
        PlayerPrefs.DeleteKey(gameObject.name + "DistanceToTravel");
        PlayerPrefs.DeleteKey(gameObject.name + "DistanceTraveled");
        PlayerPrefs.DeleteKey(gameObject.name + "OriginalDistance");

        questManager.RemoveQuest(gameObject);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void SaveQuest()
    {
        // Save quest progress
        PlayerPrefs.SetFloat(gameObject.name + "DistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat(gameObject.name + "DistanceTraveled", distanceTraveled);
        PlayerPrefs.SetFloat(gameObject.name + "OriginalDistance", originalDistance);
        PlayerPrefs.Save(); // Persist changes
    }
}
