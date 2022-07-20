using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject[] Waypoint;

    int current = 0;
    public float speed;
    float WPRadius = 1;

    void Update()
    {
        if(Vector3.Distance(Waypoint[current].transform.position, transform.position) < WPRadius)
        {
            current = Random.Range(0, Waypoint.Length);
        }
        transform.position = Vector3.MoveTowards(transform.position, Waypoint[current].transform.position, Time.deltaTime * speed);
    }
}
