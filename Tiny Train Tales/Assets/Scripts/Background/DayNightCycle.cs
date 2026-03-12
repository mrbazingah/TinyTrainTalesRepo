using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float dayNightDuration;
    [SerializeField] float morningEveningDuration;
    [SerializeField] float transitionDuration = 3f;

    [Header("Time of Day Colors")]
    [SerializeField] Color dayColor = Color.white;
    [SerializeField] Color morningColor = new Color(0.9f, 0.85f, 0.75f, 1f);
    [SerializeField] Color eveningColor = new Color(0.7f, 0.6f, 0.5f, 1f);
    [SerializeField] Color nightColor = new Color(0.3f, 0.3f, 0.5f, 1f);

    float currentDayNightDuration;
    float currentMorningEveningDuration;

    bool isTransitioning;
    float transitionProgress;
    TimeOfDay nextTime;

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

    public Color GetBlendedColor()
    {
        if (!isTransitioning)
            return GetTimeColor(currentTime);
        return Color.Lerp(GetTimeColor(currentTime), GetTimeColor(nextTime), transitionProgress);
    }

    public bool IsTransitioning() => isTransitioning;
    public float GetTransitionProgress() => transitionProgress;
    public TimeOfDay GetNextTime() => nextTime;

    public enum TimeOfDay
    {
        Morning,
        Day,
        Evening,
        Night,
    }

    void Awake()
    {
        LoadDayNightData();
    }

    public void LoadDayNightData()
    {
        DayNightSaveData data = SaveSystem.Instance.GetDayNightData();
        if (data == null) return;
        currentTime = (TimeOfDay)data.currentTime;
        currentDayNightDuration = data.currentDayNightDuration;
        currentMorningEveningDuration = data.currentMorningEveningDuration;
    }

    void Start()
    {
        if (currentDayNightDuration <= 0f)
            currentDayNightDuration = dayNightDuration;
        if (currentMorningEveningDuration <= 0f)
            currentMorningEveningDuration = morningEveningDuration;
    }

    void Update()
    {
        if (isTransitioning)
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            if (transitionProgress >= 1f)
            {
                transitionProgress = 0f;
                currentTime = nextTime;
                isTransitioning = false;
            }
            return;
        }

        if (currentTime == TimeOfDay.Morning || currentTime == TimeOfDay.Evening)
        {
            currentMorningEveningDuration -= Time.deltaTime;
            if (currentMorningEveningDuration <= 0f)
            {
                currentMorningEveningDuration = morningEveningDuration;
                StartTransition(currentTime == TimeOfDay.Morning ? TimeOfDay.Day : TimeOfDay.Night);
            }
        }
        else
        {
            currentDayNightDuration -= Time.deltaTime;
            if (currentDayNightDuration <= 0f)
            {
                currentDayNightDuration = dayNightDuration;
                StartTransition(currentTime == TimeOfDay.Day ? TimeOfDay.Evening : TimeOfDay.Morning);
            }
        }
    }

    void StartTransition(TimeOfDay to)
    {
        isTransitioning = true;
        transitionProgress = 0f;
        nextTime = to;
    }

    public void SaveDayNightData()
    {
        DayNightSaveData data = new DayNightSaveData
        {
            currentTime = (int)currentTime,
            currentDayNightDuration = currentDayNightDuration,
            currentMorningEveningDuration = currentMorningEveningDuration
        };

        SaveSystem.Instance.SetDayNightData(data);
    }
}
