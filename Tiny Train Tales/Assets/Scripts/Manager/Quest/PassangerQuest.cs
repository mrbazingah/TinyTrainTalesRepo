using TMPro;
using UnityEngine;

public class PassangerQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questText;
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
            questText.text = toDrop.ToString() + "/" + toDrop.ToString() + " Passangers delieverd";
            DeleteKeys();
        }
        else
        {
            questText.text = dropped.ToString() + "/" + toDrop.ToString();
        }
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

        questText.text = dropped.ToString() + "/" + toDrop.ToString();
    }

    void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(gameObject.name + "Dropped");
        PlayerPrefs.DeleteKey(gameObject.name + "ToDrop");
    }

    public void SavePassangers()
    {
        PlayerPrefs.SetInt(gameObject.name + "Dropped", dropped);
        PlayerPrefs.SetInt(gameObject.name + "ToDrop", toDrop);
    }
}
