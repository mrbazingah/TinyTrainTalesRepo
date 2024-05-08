using UnityEngine;

[System.Serializable]
public class TrainData
{
    public float acceleration;
    public float speed;

    public TrainData(Train train)
    {
        acceleration = train.GetAcceleration();
        speed = train.GetSpeed();   
    }
}
