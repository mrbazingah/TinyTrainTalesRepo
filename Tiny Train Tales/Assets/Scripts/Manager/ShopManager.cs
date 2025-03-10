using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    [SerializeField] Button resetButton;
    [SerializeField] Color cantBuyColor;
    [SerializeField] Color originalColor;
    [SerializeField] float resetGemCost;

    ColorBlock buttonColorBlock;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        UpdateButtons();   
    }

    void UpdateButtons()
    {
        Color buttonColor = originalColor;

        float gems = gameManager.GetGems();
        if (gems < resetGemCost)
        {
            buttonColor = cantBuyColor;
        }

        buttonColorBlock = resetButton.colors;
        buttonColorBlock.normalColor = buttonColor;
        buttonColorBlock.highlightedColor = buttonColor;
        buttonColorBlock.selectedColor = buttonColor;
        resetButton.colors = buttonColorBlock;
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
