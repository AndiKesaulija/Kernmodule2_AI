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
    public static int maxArea = 60;

    private Vector3 waypoint = new Vector3(0,0,0);

    public bool trackCohesion;
    public bool trackSeperation;
    public bool trackAlignment;
    public bool trackBreakOff;

    public bool trackWaypoint;



    [Range(1.0f, 10.0f)]
    public float avoidRange;
    




    void Start()
    {
        boids = new GameObject[maxBoids];

        Camera.main.orthographicSize = maxArea;
        GameObject Floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Floor.transform.localScale = new Vector3(maxArea, 1, maxArea);

        for (int i = 0; i < maxBoids; i++)
        {
            boids[i] = Instantiate(boid, new Vector3(Random.Range(-maxArea, maxArea), 0, Random.Range(-maxArea, maxArea)), Quaternion.identity);
        }
    }

    void Update()
    {
        MoveBoids();

        if (Input.GetMouseButtonDown(0))
        {
            waypoint = CastRay();
        }
    }
    public void MoveBoids()
    {
        foreach (GameObject boid in boids)
        {
            boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + HandleBoids(boid);

            if (trackBreakOff == true) { BreakOff(boid); }
            LimitVelocity(boid);

            boid.transform.rotation = Quaternion.LookRotation(boid.GetComponent<Rigidbody>().velocity);

            //Villager behavior
            //boid.GetComponent<Rigidbody>().velocity = boid.GetComponent<Rigidbody>().velocity + (Waypoint(boid) + Avoide(boid)+ Align(boid));
        }
    }

    Vector3 HandleBoids(GameObject b)
    {
        Vector3 totalVel = new Vector3(0, 0, 0);

        if(trackCohesion == true) { totalVel += Cohesion(b); }
        if(trackAlignment == true) { totalVel += Alignment(b); }
        if(trackSeperation == true) { totalVel += Separation(b); }
        if(trackWaypoint == true) { totalVel += Waypoint(b); }


        return totalVel;
    }
    Vector3 Cohesion(GameObject b) 
    {
        Vector3 center = new Vector3(0, 0, 0);
        int count = 1;
        foreach (GameObject boid in boids)
        {
            if (Vector3.Distance(boid.transform.position, b.transform.position) < avoidRange*5)
            {
                count += 1;
                center += boid.transform.position;
            }
        }
        center = center / count;

        //Debug.DrawLine(b.transform.position, center, Color.white, 0.01f);
        return (center - b.transform.position) / 100;
    }
    Vector3 Separation(GameObject b)
    {
        Vector3 avoide = new Vector3(0, 0, 0);

        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                //d = ((x2 - x1)2 + (y2 - y1)2 + (z2 - z1)2)1/2  
                if (Vector3.Distance(boid.transform.position, b.transform.position) < avoidRange)
                {
                    avoide += b.transform.position - boid.transform.position;
                    //Debug.DrawLine(b.transform.position, boid.transform.position, Color.red, 0.01f);
                }
            }
        }
        return avoide;
    }
    Vector3 Alignment(GameObject b)
    {
        Vector3 speed = new Vector3(0, 0, 0);
        int count = 1;
        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                if (Vector3.Distance(boid.transform.position, b.transform.position) < (avoidRange *2))
                {
                    count += 1;
                    speed = speed + boid.GetComponent<Rigidbody>().velocity;
                }
            }
        }
        speed = speed/count;

        return speed / 18;
    }

    void BreakOff(GameObject b)
    {
        Vector3 dash = new Vector3(0, 0, 0);
        int count = 1;

        List<GameObject> boidGroup = new List<GameObject>(); 

        foreach (GameObject boid in boids)
        {
            if (boid != b)
            {
                if (Vector3.Distance(boid.transform.position, b.transform.position) < (avoidRange * 2))
                {
                    count += 1;
                    dash += (b.transform.position - boid.transform.position);
                    boidGroup.Add(boid);

                    Debug.DrawLine(b.transform.position, boid.transform.position, Color.red, 0.01f);
                }
            }
        }

        if (count > 5)
        {
            foreach (GameObject boid in boidGroup)
            {
                boid.GetComponent<Rigidbody>().velocity += (boid.GetComponent<Rigidbody>().velocity + dash) / 10;
            }
        }
    }
    void LimitVelocity(GameObject b)
    {
        float maxSpeed = 10;
        if(b.GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
        {
            b.GetComponent<Rigidbody>().velocity = b.GetComponent<Rigidbody>().velocity - (b.GetComponent<Rigidbody>().velocity / 2);
        }
    }

    Vector3 Waypoint(GameObject b)
    {
        return (waypoint - b.transform.position) / 10;
    }
    Vector3 CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.GetPoint(20), Color.blue, 4);
        Debug.Log(ray.GetPoint(20));
        return new Vector3(ray.GetPoint(20).x, 0, ray.GetPoint(20).z);
    }

   
}
