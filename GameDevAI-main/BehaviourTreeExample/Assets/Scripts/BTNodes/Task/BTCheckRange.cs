using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTCheckRange : BTBaseNode
{
    private VariableGameObject myAgent;
    private VariableGameObject target;
    private float maxRange;



    public BTCheckRange(VariableGameObject myAgent, VariableGameObject target, float maxRange)
    {
        this.myAgent = myAgent;
        this.target = target;
        this.maxRange = maxRange;
    }
    public override TaskStatus Run()
    {
        if(Vector3.Distance(myAgent.Value.transform.position, target.Value.transform.position) > maxRange)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failed;
        }
    }
}
