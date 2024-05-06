using UnityEngine;

[System.Serializable]
public class TrainData
{
    public float acceleration;
    public float decelartion;
    public float speed;

    public TrainData(Train train)
    {
        acceleration = train.GetAcceleration();
        decelartion = train.GetDecelartion();
        speed = train.GetSpeed();   
    }
}
