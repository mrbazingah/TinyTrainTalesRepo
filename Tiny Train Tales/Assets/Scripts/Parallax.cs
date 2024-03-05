using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxEffect;
    [SerializeField] GameObject camera;

    float length;
    float startPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = camera.transform.position.x * (1 - parallaxEffect);
        float dist = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector2(startPos + dist, transform.position.y);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
