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
            Debug.LogWarning("No saved time found for " + saveKey);
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
        Debug.Log("Time saved: " + DateTime.Now);
    }
}
