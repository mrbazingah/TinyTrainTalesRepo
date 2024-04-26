using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrain : MonoBehaviour
{
    [SerializeField] GameObject endCity;
    [SerializeField] float speedOffset;

    float speed;

    Train train;

    void Awake()
    {
        train = FindObjectOfType<Train>();
    }

    void FixedUpdate()
    {
        speed = train.GetVelocity() / speedOffset / 4f;

        transform.position = Vector3.MoveTowards(transform.position, endCity.transform.position, speed);
    }
}
