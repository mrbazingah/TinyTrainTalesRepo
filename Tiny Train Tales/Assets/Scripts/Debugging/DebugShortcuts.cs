using UnityEngine;

public class DebugShortcuts : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Value Keybinds")]
    [SerializeField] KeyCode coinsShortcut;
    [SerializeField] KeyCode gemsShortcut;
    [SerializeField] KeyCode speedShortcut;
    [SerializeField] KeyCode accelerationShortcut;

    [Header("Add or Remove Keybinds")]
    [SerializeField] KeyCode increaseShortcut;
    [SerializeField] KeyCode decreaseShortcut;

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
        if (Input.GetKey(coinsShortcut))
        {
            if (Input.GetKeyDown(increaseShortcut))
            {
                gameManager.AddCoins(coinsValue);
                Debug.Log($"[DebugShortcuts] Added {coinsValue} Coins * (Profit modifier)");
            }
            
            if (Input.GetKeyDown(decreaseShortcut))
            {
                gameManager.AddCoins(-coinsValue);
                Debug.Log($"[DebugShortcuts] Removed {coinsValue} Coins * (Profit modifier)");
            }
        }
    }

    void Gems()
    {
        if (Input.GetKey(gemsShortcut))
        {
            if (Input.GetKeyDown(increaseShortcut))
            {
                gameManager.AddGems(gemsValue);
                Debug.Log($"[DebugShortcuts] Added {gemsValue} Gems");
            }

            if (Input.GetKeyDown(decreaseShortcut))
            {
                gameManager.AddGems(-gemsValue);
                Debug.Log($"[DebugShortcuts] Removed {gemsValue} Gems");
            }
        }
    }

    void Speed()
    {
        if (Input.GetKey(speedShortcut))
        {
            if (Input.GetKeyDown(increaseShortcut))
            {
                gameManager.AddToMaxSpeed(speedValue);
                Debug.Log($"[DebugShortcuts] Added {speedValue} to MaxSpeed");
            }

            if (Input.GetKeyDown(decreaseShortcut))
            {
                gameManager.AddToMaxSpeed(-speedValue);
                Debug.Log($"[DebugShortcuts] Removed {speedValue} from MaxSpeed");
            }
        }
    }

    void Acceleration()
    {
        if (Input.GetKey(accelerationShortcut))
        {
            if (Input.GetKeyDown(increaseShortcut))
            {
                train.AddToAcceleration(accelerationValue);
                Debug.Log($"[DebugShortcuts] Added {accelerationValue} to Acceleration");
            }

            if (Input.GetKeyDown(decreaseShortcut))
            {
                train.AddToAcceleration(-accelerationValue);
                Debug.Log($"[DebugShortcuts] Removed {accelerationValue} from Acceleration");
            }
        }
    }
#endif
}
