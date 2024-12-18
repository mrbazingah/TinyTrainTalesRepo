using TMPro;
using UnityEngine;

public class PassangerQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] GameObject collectButton;
    [SerializeField] float passangersToDropOff; // Total distance needed to complete the quest
    [SerializeField] float passangersDroppedOff; // Distance already traveled
    [SerializeField] float minPassangers, maxPassangers;
    [SerializeField] float multiplier;
    [SerializeField] bool getGems; // Specify if this quest gives gems as a reward

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
        if (PlayerPrefs.HasKey(gameObject.name + "PassangersToDropOff"))
        {
            // Load saved quest data
            passangersToDropOff = PlayerPrefs.GetFloat(gameObject.name + "PassangersToDropOff");
            passangersDroppedOff = PlayerPrefs.GetFloat(gameObject.name + "PassangersDroppedOff");
        }
        else
        {
            passangersToDropOff = (int)Random.Range(minPassangers, maxPassangers + 1) * multiplier;
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
            reward = passangersToDropOff / 100;
            gameManager.AddGems(reward);
        }
        else
        {
            reward = passangersToDropOff * 10;
            gameManager.AddCoins(reward);
        }

        ResetQuest();
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