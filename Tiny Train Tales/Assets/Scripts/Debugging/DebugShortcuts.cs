using UnityEngine;

public class DebugShortcuts : MonoBehaviour
{
    [Header("Value Keybinds")]
    [SerializeField] KeyCode coinsKey;
    [SerializeField] KeyCode gemsKey;
    [SerializeField] KeyCode speedKey;
    [SerializeField] KeyCode accelerationKey;

    [Header("Add or Remove Keybinds")]
    [SerializeField] KeyCode increaseKey;
    [SerializeField] KeyCode decreaseKey;

    [Header("Values")]
    [SerializeField] float coinsValue;
    [SerializeField] float gemsValue;
    [SerializeField] float speedValue;
    [SerializeField] float accelerationValue;

    GameManager gameManager;
    Train train;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        train = FindObjectOfType<Train>();
    }

    void Update()
    {
        Coins();
        Gems();
        Speed();
        Acceleration();
    }

    void Coins()
    {
        if (Input.GetKey(coinsKey))
        {
            if (Input.GetKeyDown(increaseKey))
            {
                gameManager.AddCoins(coinsValue);
                Debug.Log($"[DebugShortcuts] Added {coinsValue} Coins * (Profit modifier)");
            }
            
            if (Input.GetKeyDown(decreaseKey))
            {
                gameManager.AddCoins(-coinsValue);
                Debug.Log($"[DebugShortcuts] Removed {coinsValue} Coins * (Profit modifier)");
            }
        }
    }

    void Gems()
    {
        if (Input.GetKey(gemsKey))
        {
            if (Input.GetKeyDown(increaseKey))
            {
                gameManager.AddGems(gemsValue);
                Debug.Log($"[DebugShortcuts] Added {gemsValue} Gems");
            }

            if (Input.GetKeyDown(decreaseKey))
            {
                gameManager.AddGems(-gemsValue);
                Debug.Log($"[DebugShortcuts] Removed {gemsValue} Gems");
            }
        }
    }

    void Speed()
    {
        if (Input.GetKey(speedKey))
        {
            if (Input.GetKeyDown(increaseKey))
            {
                gameManager.AddToMaxSpeed(speedValue);
                Debug.Log($"[DebugShortcuts] Added {speedValue} to MaxSpeed");
            }

            if (Input.GetKeyDown(decreaseKey))
            {
                gameManager.AddToMaxSpeed(-speedValue);
                Debug.Log($"[DebugShortcuts] Removed {speedValue} from MaxSpeed");
            }
        }
    }

    void Acceleration()
    {
        if (Input.GetKey(accelerationKey))
        {
            if (Input.GetKeyDown(increaseKey))
            {
                train.AddToAcceleration(accelerationValue);
                Debug.Log($"[DebugShortcuts] Added {accelerationValue} to Acceleration");
            }

            if (Input.GetKeyDown(decreaseKey))
            {
                train.AddToAcceleration(-accelerationValue);
                Debug.Log($"[DebugShortcuts] Removed {accelerationValue} from Acceleration");
            }
        }
    }
}
