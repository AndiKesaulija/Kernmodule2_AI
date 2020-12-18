using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 waypoint;

    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleBoid();
    }

    public void SetNewWaypoint(float size)
    {
        waypoint = new Vector3(Random.Range(-size, size), 0, Random.Range(-size, size));
    }

    public void HandleBoid()
    {
        if (transform.position.x > SpawnBoids.maxArea * 2)
        {
            transform.position = new Vector3(-SpawnBoids.maxArea * 2, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -SpawnBoids.maxArea * 2)
        {
            transform.position = new Vector3(SpawnBoids.maxArea *2, transform.position.y, transform.position.z);
        }
        if (transform.position.z > SpawnBoids.maxArea)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -SpawnBoids.maxArea);
        }
        if (transform.position.z < -SpawnBoids.maxArea)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, SpawnBoids.maxArea);
        }
    }



   
}
