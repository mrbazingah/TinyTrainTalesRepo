using TMPro;
using UnityEngine;

public class BuyRestrictions : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] TextMeshProUGUI amountText;

    int currentAmount;

    void Start()
    {
        currentAmount = PlayerPrefs.GetInt(gameObject.name + "Amount");
        amountText.text = currentAmount.ToString() + "/" + amount.ToString(); 
    }

    public void AddAmount()
    {
        if (currentAmount == amount) { return; }

        currentAmount++;

        amountText.text = currentAmount.ToString() + "/" + amount.ToString();
        PlayerPrefs.SetInt(gameObject.name + "Amount", currentAmount);
    }
}
