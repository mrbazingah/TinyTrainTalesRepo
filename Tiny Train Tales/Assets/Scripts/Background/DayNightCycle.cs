using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float dayNightTime;
    [SerializeField] float morningEveningTime;

    float currentDayNightTime;
    float currentMorningEveningTime;

    public DayTime currentTime;

    public enum DayTime
    {
        Morning,
        Day,
        Evening,
        Night,
    }

    void Start()
    {
        currentDayNightTime = dayNightTime;
        currentMorningEveningTime = morningEveningTime;
    }

    void Update()
    {
        
    }
}
