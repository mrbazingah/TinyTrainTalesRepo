using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    // Load the saved time from PlayerPrefs (returns DateTime.MinValue if none saved)
    public DateTime LoadPastTime(string saveKey)
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            long binaryTime = Convert.ToInt64(PlayerPrefs.GetString(saveKey));
            DateTime savedTime = DateTime.FromBinary(binaryTime);
            return savedTime;
        }
        else
        {
            return DateTime.MinValue;
        }
    }

    // Returns true if the given number of hours have passed since the saved time
    public bool GetCurrentTime(float hours, string saveKey)
    {
        DateTime savedTime = LoadPastTime(saveKey);
        if (savedTime == DateTime.MinValue) return false; // nothing saved yet

        TimeSpan timePassed = DateTime.Now - savedTime;
        return timePassed.TotalHours >= hours;
    }

    // Save the current time as a string in PlayerPrefs
    public void SaveCurrentTime(string saveKey)
    {
        string currentTime = DateTime.Now.ToBinary().ToString();
        PlayerPrefs.SetString(saveKey, currentTime);
        PlayerPrefs.Save();
    }

    public TimeSpan GetTimeUntilReset(float hours, string saveKey)
    {
        DateTime savedTime = LoadPastTime(saveKey);
        if (savedTime == DateTime.MinValue)
            return TimeSpan.Zero; // nothing saved yet

        TimeSpan elapsed = DateTime.Now - savedTime;
        TimeSpan totalDuration = TimeSpan.FromHours(hours);
        TimeSpan remaining = totalDuration - elapsed;

        // Prevent negative results
        if (remaining.TotalSeconds < 0)
            remaining = TimeSpan.Zero;

        return remaining;
    }

}
