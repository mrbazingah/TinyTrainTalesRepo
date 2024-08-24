using TMPro;
using UnityEngine;

public class PassangerQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
    [SerializeField] GameObject icon;
    [SerializeField] GameObject collectButton;
    [SerializeField] int maxPassangers;
    [SerializeField] int minPassangers;
    [SerializeField] int multiply = 10;

    int dropped = 0;
    int toDrop;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();  
    }

    void Start()
    {
        SetUpQuest();
    }

    public void DropOff()
    {
        dropped += gameManager.GetSubPassangers();

        if (dropped >= toDrop)
        {
            questText.text = "Complete";

            icon.SetActive(false);
            collectButton.SetActive(true);
        }
        else
        {
            questText.text = dropped.ToString() + "/" + toDrop.ToString();
        }

        PlayerPrefs.SetInt(gameObject.name + "Dropped", dropped);
        toDrop = PlayerPrefs.GetInt(gameObject.name + "ToDrop");
    }

    void SetUpQuest()
    {
        if (PlayerPrefs.HasKey(gameObject.name + "Dropped"))
        {
            dropped = PlayerPrefs.GetInt(gameObject.name + "Dropped");
            toDrop =  PlayerPrefs.GetInt(gameObject.name + "ToDrop");
        }
        else
        {
            toDrop = Random.Range(minPassangers, maxPassangers + 1) * multiply;
        }

        if (dropped >= toDrop)
        {
            questText.text = "Complete";

            icon.SetActive(false);
            collectButton.SetActive(true);
        }
        else
        {
            questText.text = dropped.ToString() + "/" + toDrop.ToString();
            collectButton.SetActive(false);
        }
    }

    public void SavePassangers()
    {
        PlayerPrefs.SetInt(gameObject.name + "Dropped", dropped);
        PlayerPrefs.SetInt(gameObject.name + "ToDrop", toDrop);
    }

    public void Collect()
    {
        gameManager.AddToGems(toDrop / 10);
        DeleteKeys();
        Destroy(gameObject);
    }

    void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(gameObject.name + "Dropped");
        PlayerPrefs.DeleteKey(gameObject.name + "ToDrop");
    }
}
