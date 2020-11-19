using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnBoids : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] boids;

    public GameObject boid;
    public GameObject centerObject;

    public Camera mainCamera;

    public int maxBoids;
    public int maxArea;


    void Start()
    {
        boids = new GameObject[maxBoids];

        mainCamera.orthographicSize = maxArea;

        for (int i = 0; i < maxBoids; i++)
        {
            boids[i] = Instantiate(boid, new Vector3(Random.Range(-maxArea, maxArea), 0, Random.Range(-maxArea, maxArea)), Quaternion.identity);
        }
    }

    void Update()
    {
        MoveBoids();
        HandleBoids();

        if (Input.GetMouseButtonDown(0))
        {
            WayPoint();
        }
    }
    public void MoveBoids()
    {

        foreach (GameObject boid in boids)
        {
            boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + Center(boid) + Avoide(boid) + Align(boid);
        }

    }
    public void HandleBoids()
    {
        foreach (GameObject boid in boids)
        {
            if (boid.transform.position.x > maxArea || boid.transform.position.x < -maxArea)
            {
                boid.transform.position = new Vector3(-boid.transform.position.x, boid.transform.position.y, boid.transform.position.z);
            }
            if (boid.transform.position.y > maxArea || boid.transform.position.y < -maxArea)
            {
                boid.transform.position = new Vector3(boid.transform.position.x, -boid.transform.position.y, boid.transform.position.z);
            }
            if (boid.transform.position.z > maxArea || boid.transform.position.z < -maxArea)
            {
                boid.transform.position = new Vector3(boid.transform.position.x, boid.transform.position.y, -boid.transform.position.z);
            }
        }
    }
    Vector3 Center(GameObject b) 
    {
        Vector3 center = new Vector3(0, 0, 0);
        foreach (GameObject boid in boids)
        {
            if(boid != b)
            {
                center += boid.transform.position;
            }
        }

        center = center / (maxBoids - 1);
        Debug.DrawLine(center, b.transform.position, Color.white, 0.01f);

        return (center - b.transform.position) / 100;
    }
    Vector3 Avoide(GameObject b)
    {
        Vector3 c = Vector3.zero;

        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                if(Vector3.Distance(boid.transform.position, b.transform.position) < 5)
                {
                    //d = ((x2 - x1)2 + (y2 - y1)2 + (z2 - z1)2)1/2  
                    //Debug.Log("Dist: " + Vector3.Distance(boid.transform.position, b.transform.position));

                    c =  c - (boid.transform.position - b.transform.position);
                    Debug.DrawLine(boid.transform.position, b.transform.position, Color.red, 0.01f);
                }
            }
        }
        return c;
    }
    Vector3 Align(GameObject b)
    {
        Vector3 speed = new Vector3(0, 0, 0);
        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                speed = speed + boid.GetComponent<Rigidbody>().velocity;
            }
        }

        speed = speed / (maxBoids - 1);

        return (speed - b.GetComponent<Rigidbody>().velocity) / 200;
    }
    Vector3 WayPoint()
    {
        Vector3 waypoint = Vector3.zero;

        foreach(GameObject boid in boids)
        {
            boid.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        return waypoint;
    }

    
  
   


}
