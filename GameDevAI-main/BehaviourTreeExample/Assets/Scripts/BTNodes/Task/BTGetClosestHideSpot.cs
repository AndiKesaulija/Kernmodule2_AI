using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;


public class BTGetClosestHideSpot : BTBaseNode
{
    private List<VariableGameObject> objects;
    private VariableGameObject myAgent;
    private VariableGameObject myTarget;
    private VariableGameObject myEnemy;

    private float[] RangeList;
    private float [] sortedRangeList;

    private List<VariableGameObject> sortedObjects;

    public BTGetClosestHideSpot(List<VariableGameObject> objects, VariableGameObject myAgent, VariableGameObject myTarget, VariableGameObject myEnemy)
    {
        this.objects = objects;
        this.myAgent = myAgent;
        this.myTarget = myTarget;
        this.myEnemy = myEnemy;

        RangeList = new float[objects.Count];
        sortedRangeList = new float[objects.Count];

        sortedObjects = new List<VariableGameObject>();
    }

    public override TaskStatus Run()
    {


        for (int i = 0; i < objects.Count; i++)
        {
            RangeList[i] = Vector3.Distance(objects[i].Value.transform.position, myAgent.Value.transform.position);
        }

        sortedRangeList = RangeList.ToArray();
        Array.Sort(sortedRangeList);

        

        for (int i = 0; i < sortedRangeList.Length; i++)
        {
            int minIndex = RangeList.ToList().IndexOf(sortedRangeList[i]);
            sortedObjects.Add(objects[minIndex]);
        }

        for (int i = 0; i < sortedObjects.Count; i++)
        {
            if (new BTSpot(myEnemy, sortedObjects[i], 200, false).Run() == TaskStatus.Failed)
            {
                myTarget.Value = sortedObjects[i].Value;
                sortedObjects.Clear();
                return TaskStatus.Success;
            }

        }

        sortedObjects.Clear();
        Debug.Log("NoHidingSpot");
        return TaskStatus.Failed;
    }
    
}
