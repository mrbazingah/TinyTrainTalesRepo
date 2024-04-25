using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrain : MonoBehaviour
{
    [SerializeField] GameObject mapTrain;
    [SerializeField] GameObject[] cites;

    float speed;

    Train train;

    void Awake()
    {
        train = FindObjectOfType<Train>();
    }

    void Update()
    {
        speed = train.GetVelocity() * 4.3f;
    }
}
