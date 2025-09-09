using TMPro;
using UnityEngine;

public class PassangerQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] GameObject collectButton;
    [Space]
    [SerializeField] float passangersToDropOff; 
    [SerializeField] float passangersDroppedOff; 
    [SerializeField] float minPassangers, maxPassangers;
    [Space]
    [SerializeField] float minPassangerForMultiplier;
    [SerializeField] float multiplier;
    [Space]
    [SerializeField] bool getGems;

    bool hasCompleted;

    GameManager gameManager;
    QuestManager questManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();
    }

    public void SetUpQuest()
    {
        string pastDate = PlayerPrefs.GetString("PastDate" + gameObject.name, "");
        string currentDate = System.DateTime.Now.ToString("yyyyMMdd");
        if (pastDate != currentDate)
        {
            if (PlayerPrefs.HasKey(gameObject.name + "DistanceToTravel"))
            {
                ResetQuest();
            }

            PlayerPrefs.SetString("PastDate" + gameObject.name, currentDate);
        }
        else if (PlayerPrefs.HasKey("QuestCompleted" + gameObject.name))
        {
            UpdateQuestVisuals();
            hasCompleted = true;
            return;
        }

        if (PlayerPrefs.HasKey(gameObject.name + "PassangersToDropOff"))
        {
            // Load saved quest data
            passangersToDropOff = PlayerPrefs.GetFloat(gameObject.name + "PassangersToDropOff");
            passangersDroppedOff = PlayerPrefs.GetFloat(gameObject.name + "PassangersDroppedOff");

            if (passangersDroppedOff >= passangersToDropOff)
            {
                passangersDroppedOff = passangersToDropOff;
                CompleteQuest();
            }
            else
            {
                collectButton.SetActive(false);
            }
        }
        else
        {
            // Initialize new quest
            float passnagerMultiplier = gameManager.GetMaxPassangers() < minPassangerForMultiplier ? 10 : multiplier;
            passangersToDropOff = (int)Random.Range(minPassangers, maxPassangers + 1) * passnagerMultiplier;
            collectButton.SetActive(false);

            getGems = ((int)Random.Range(0, 2) == 0) ? true : false;
        }

        questText.text = passangersDroppedOff.ToString() + "/" + passangersToDropOff.ToString();
    }

    public void TrackPassangers(float passangers)
    {
        if (hasCompleted) { return; }

        passangersDroppedOff += passangers;

        if (passangersDroppedOff >= passangersToDropOff)
        {
            passangersDroppedOff = passangersToDropOff;
            CompleteQuest();
        }

        SaveQuest();

        questText.text = passangersDroppedOff.ToString() + "/" + passangersToDropOff.ToString();
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
            reward = passangersToDropOff / 10;
            Mathf.Clamp(reward, 1, 100);
            gameManager.AddGems(reward);
        }
        else
        {
            reward = passangersToDropOff * 10;
            gameManager.AddCoins(reward);
        }

        PlayerPrefs.SetInt("QuestCompleted" + gameObject.name, 1);
        UpdateQuestVisuals();
    }

    void UpdateQuestVisuals()
    {
        collectButton.SetActive(false);
        questText.text = "Completed!";
    }

    void ResetQuest()
    {
        // Clear saved progress and destroy this quest object
        PlayerPrefs.DeleteKey(gameObject.name + "PassangersToDropOff");
        PlayerPrefs.DeleteKey(gameObject.name + "PassangersDroppedOff");

        questManager.RemoveQuest(gameObject);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void SaveQuest()
    {
        // Save quest progress
        PlayerPrefs.SetFloat(gameObject.name + "PassangersToDropOff", passangersToDropOff);
        PlayerPrefs.SetFloat(gameObject.name + "PassangersDroppedOff", passangersDroppedOff);
        PlayerPrefs.Save(); // Persist changes
    }
}