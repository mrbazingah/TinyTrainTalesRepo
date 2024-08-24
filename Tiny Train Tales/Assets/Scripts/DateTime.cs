using UnityEngine;

public class DateTime : MonoBehaviour
{
    void Awake()
    {
        int pastDay = PlayerPrefs.GetInt("PastDay");
        int pasthMonth = PlayerPrefs.GetInt("PastMonth");
        int pastYear = PlayerPrefs.GetInt("PastYear");

        int currentDay = System.DateTime.Now.Day;
        int currentMonth = System.DateTime.Now.Month;
        int currentYear = System.DateTime.Now.Year;

        if (pastDay < currentDay || pasthMonth < currentMonth || pastYear < currentYear)
        {
            PlayerPrefs.SetInt("PastDay", currentDay);
            PlayerPrefs.SetInt("PastMonth", currentMonth);
            PlayerPrefs.SetInt("PastYear", currentYear);
        }
    }
}
