using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnBoids : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] boids;

    public GameObject boid;

    public int maxBoids;
    public int maxArea;


    [Range(1.0f,200.0f)]
    public float centerSpeed = 200;
    [Range(1.0f, 10.0f)]
    public float avoidRange = 5;

    private Vector3 waypoint = new Vector3(0,0,0);


    void Start()
    {
        boids = new GameObject[maxBoids];

        Camera.main.orthographicSize = maxArea / 2;
        GameObject Floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Floor.transform.localScale = new Vector3(4, 1, 4);
        

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
            waypoint = CastRay();
        }
    }
    public void MoveBoids()
    {

        foreach (GameObject boid in boids)
        {
            //boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + Center(boid) + Avoide(boid) + Align(boid);
            boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + Avoide(boid) + Waypoint(boid);
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

        return (center - b.transform.position) / centerSpeed;
    }

    Vector3 Avoide(GameObject b)
    {
        Vector3 c = Vector3.zero;

        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                if(Vector3.Distance(boid.transform.position, b.transform.position) < avoidRange)
                {
                    //d = ((x2 - x1)2 + (y2 - y1)2 + (z2 - z1)2)1/2  
                    //Debug.Log("Dist: " + Vector3.Distance(boid.transform.position, b.transform.position));

                    c =  c - (boid.transform.position - b.transform.position) /10;
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
    Vector3 Waypoint(GameObject b)
    {
        Vector3 center = waypoint;

        if (Vector3.Distance(b.transform.position, center) > avoidRange)
        {

            return (center - b.transform.position) / centerSpeed;
        }

        Debug.DrawLine(center, b.transform.position, Color.white, 0.01f);

        return Vector3.zero;
    }
    Vector3 CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.GetPoint(20),Color.blue,4);
        Debug.Log(ray.GetPoint(20));
        return new Vector3(ray.GetPoint(20).x,0, ray.GetPoint(20).z);
    }
    

    
  
   


}
