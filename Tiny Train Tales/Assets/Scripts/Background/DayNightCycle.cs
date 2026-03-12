using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float dayNightDuration;
    [SerializeField] float morningEveningDuration;

    [Header("Time of Day Colors")]
    [SerializeField] Color dayColor = Color.white;
    [SerializeField] Color morningColor = new Color(0.9f, 0.85f, 0.75f, 1f);
    [SerializeField] Color eveningColor = new Color(0.7f, 0.6f, 0.5f, 1f);
    [SerializeField] Color nightColor = new Color(0.3f, 0.3f, 0.5f, 1f);

    float currentDayNightDuration;
    float currentMorningEveningDuration;

    public TimeOfDay currentTime;

    public Color GetTimeColor(TimeOfDay time)
    {
        return time switch
        {
            TimeOfDay.Morning => morningColor,
            TimeOfDay.Day     => dayColor,
            TimeOfDay.Evening => eveningColor,
            TimeOfDay.Night   => nightColor,
            _                 => dayColor,
        };
    }

    public enum TimeOfDay
    {
        Morning,
        Day,
        Evening,
        Night,
    }

    void Start()
    {
        currentDayNightDuration = dayNightDuration;
        currentMorningEveningDuration = morningEveningDuration;
    }

    void Update()
    {
        if (currentTime == TimeOfDay.Morning || currentTime == TimeOfDay.Evening)
        {
            currentMorningEveningDuration -= Time.deltaTime;
            if (currentMorningEveningDuration <= 0f)
            {
                currentMorningEveningDuration = morningEveningDuration;
                currentTime = currentTime == TimeOfDay.Morning ? TimeOfDay.Day : TimeOfDay.Night;
            }
        }
        else
        {
            currentDayNightDuration -= Time.deltaTime;
            if (currentDayNightDuration <= 0f)
            {
                currentDayNightDuration = dayNightDuration;
                currentTime = currentTime == TimeOfDay.Day ? TimeOfDay.Evening : TimeOfDay.Morning;
            }
        }
    }
}
