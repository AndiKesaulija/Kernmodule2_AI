using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTNextWaypoint : BTBaseNode
{
    private VariableGameObject currentWaypoint;
    private List<GameObject> waypoints;

    public BTNextWaypoint(VariableGameObject currentWaypoint , List<GameObject> waypoints)
    {
        this.currentWaypoint = currentWaypoint;
        this.waypoints = waypoints;
    }
    public override TaskStatus Run()
    {

        int nextWaypoint = waypoints.LastIndexOf(currentWaypoint.Value) + 1;
        Debug.Log(nextWaypoint + " : " + waypoints.Count);
        
        if (nextWaypoint < waypoints.Count)
        {
            currentWaypoint.Value = waypoints[nextWaypoint];
        }
        else
        {
            currentWaypoint.Value = waypoints[0];

        }

        //Debug.Log("NextWaypoint" + " - Index: " + waypoints.LastIndexOf(currentWaypoint) + " currentWaypoint:" + currentWaypoint.Value.transform.position);
        return TaskStatus.Success;    
    }
}
