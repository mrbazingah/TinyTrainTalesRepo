using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] int resetGemCost;
    
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void ResetSlots()
    {
        float gems = gameManager.GetGems();
        if (gems < resetGemCost) { return; }

        gameManager.BuyWithGems(resetGemCost);

        for (int i = 0; i < slots.Length; i++)
        {
            Slot slotScript = slots[i].GetComponent<Slot>();
            slotScript.ResetPlayerPrefs();
            slotScript.SetUpSlot();
        }
    }
}
