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
    [SerializeField] float savedDistanceTraveled;
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
            savedDistanceTraveled = PlayerPrefs.GetFloat(gameObject.name + "DistanceTraveled");
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
        if (hasCompleted) { return; }

        // Ensure original distance is initialized
        if (originalDistance == 0)
        {
            originalDistance = gameManager.GetRemainingDistance();
        }

        // Calculate total distance covered since the quest started
        float currentRemainingDistance = gameManager.GetRemainingDistance();
        distanceTraveled = Mathf.Abs(savedDistanceTraveled + (originalDistance - currentRemainingDistance));

        // Update distanceTraveled only if totalCovered is greater than the saved value
        if (distanceTraveled < distanceToTravel)
        {
            distanceTraveled = Mathf.Min(distanceTraveled, distanceToTravel); // Cap at total distanceToTravel
            progressSlider.value = distanceTraveled;
        }

        // Calculate the remaining distance
        currentDistanceToTravel = distanceToTravel - distanceTraveled;

        // Prevent negative values
        if (currentDistanceToTravel <= 0)
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
        collectButton.SetActive(true);
        hasCompleted = true;
    }

    public void CollectReward()
    {
        float reward;
        if (getGems)
        {
            reward = distanceToTravel / 10; 
            gameManager.AddGems(reward);
        }
        else
        {
            reward = distanceToTravel; 
            gameManager.AddCoins(reward);
        }

        ResetQuest();
    }

    void ResetQuest()
    {
        // Clear saved progress and destroy this quest object
        PlayerPrefs.DeleteKey(gameObject.name + "DistanceToTravel");
        PlayerPrefs.DeleteKey(gameObject.name + "DistanceTraveled");

        questManager.RemoveQuest(gameObject);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void SaveQuest()
    {
        // Save quest progress
        PlayerPrefs.SetFloat(gameObject.name + "DistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat(gameObject.name + "DistanceTraveled", distanceTraveled);
        PlayerPrefs.Save(); // Persist changes
    }
}