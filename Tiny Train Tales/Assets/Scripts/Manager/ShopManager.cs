using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] slots;

    void Start()
    {
        SetUpCars();
    }

    void SetUpCars()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            TextMeshProUGUI weightText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            int weight = Random.Range(1, 10);
            weightText.text = "Weight: " + weight.ToString();

            TextMeshProUGUI speedText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            int speed = Random.Range(1, 10 - weight + 1);
            speedText.text = "Speed: " + speed.ToString();

            TextMeshProUGUI incomeText = slots[i].GetComponentInChildren<TextMeshProUGUI>();
            int income = Random.Range(1, 10 - speed - weight + 1);
            incomeText.text = "Income: " + income.ToString();
        }
    }
}
