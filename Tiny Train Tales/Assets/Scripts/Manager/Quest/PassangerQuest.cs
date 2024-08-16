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
        toDrop = Random.Range(minPassangers, maxPassangers + 1) * multiply;
        questText.text = dropped.ToString() + "/" + toDrop.ToString();
    }

    public void DropOff()
    {
        dropped += gameManager.GetSubPassangers();

        if (dropped >= toDrop)
        {
            //Do stuff
            questText.text = toDrop.ToString() + "/" + toDrop.ToString();
        }
        else
        {
            questText.text = dropped.ToString() + "/" + toDrop.ToString();
        }
    }
}
