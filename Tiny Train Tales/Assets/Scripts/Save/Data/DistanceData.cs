using UnityEngine;

[System.Serializable]
public class DistanceData
{
    public float distance;
    public float remainingDistance;

    public DistanceData(GameManager gameManager)
    {
        distance = gameManager.GetDistance();
        remainingDistance = gameManager.GetRemainingDistance();
    }
}
