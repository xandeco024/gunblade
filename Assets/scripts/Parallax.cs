using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float Lenght;
    private float StartPos;
    public GameObject cam;

    public float ParallaxEffect;

    void Start()
    {
        StartPos = transform.position.x;
        Lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - ParallaxEffect));
        float dist = (cam.transform.position.x * ParallaxEffect);

        transform.position = new Vector3(StartPos + dist, transform.position.y, transform.position.z);

        if (temp > StartPos + Lenght) StartPos += Lenght;
        else if (temp < StartPos - Lenght) StartPos -= Lenght;
    }
}
