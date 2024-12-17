using UnityEngine;

public class DistanceQuest : MonoBehaviour
{
    [SerializeField] float distanceToTravel;        // Total distance needed to complete the quest
    [SerializeField] float distanceTraveled;        // Distance already traveled
    [SerializeField] float maxDistance, minDistance;
    [SerializeField] float multiplier;
    [SerializeField] float currentDistanceToTravel; // Remaining distance to travel

    float orignialDistance; // Original remaining distance when the quest started

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SetUpQuest()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "DistanceToTravel"))
        {
            // Load saved quest data
            distanceToTravel = PlayerPrefs.GetFloat(gameObject.name + "DistanceToTravel");
            distanceTraveled = PlayerPrefs.GetFloat(gameObject.name + "DistanceTraveled");
            orignialDistance = PlayerPrefs.GetFloat(gameObject.name + "OriginalDistance", 0);
        }
        else
        {
            // Generate a new quest
            distanceToTravel = Random.Range(minDistance, maxDistance + 1) * multiplier;
            distanceTraveled = 0;
            orignialDistance = gameManager.GetRemainingDistance(); // Start tracking from here
        }
    }

    void Update()
    {
        TrackTravel();
    }

    void TrackTravel()
    {
        // Ensure original distance is initialized
        if (orignialDistance == 0)
        {
            orignialDistance = gameManager.GetRemainingDistance();
        }

        // Calculate total distance covered since the quest started
        float currentRemainingDistance = gameManager.GetRemainingDistance();
        float totalCovered = orignialDistance - currentRemainingDistance;

        // Increment distanceTraveled cumulatively
        float progress = totalCovered - distanceTraveled;

        if (progress > 0) // Only update if progress is positive
        {
            distanceTraveled += progress;
            distanceTraveled = Mathf.Min(distanceTraveled, distanceToTravel); // Cap at total distanceToTravel
        }

        // Calculate the remaining distance
        currentDistanceToTravel = distanceToTravel - distanceTraveled;

        // Prevent negative values
        if (currentDistanceToTravel < 0)
        {
            currentDistanceToTravel = 0;
        }
    }

    public void SaveQuest()
    {
        // Save quest progress
        PlayerPrefs.SetFloat(gameObject.name + "DistanceToTravel", distanceToTravel);
        PlayerPrefs.SetFloat(gameObject.name + "DistanceTraveled", distanceTraveled);
        PlayerPrefs.SetFloat(gameObject.name + "OriginalDistance", orignialDistance);
        PlayerPrefs.Save(); // Persist changes
    }
}
