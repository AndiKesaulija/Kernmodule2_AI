using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BTStop : BTBaseNode
{
    private VariableNavMeshAgent agent;


    public BTStop(VariableNavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override TaskStatus Run()
    {
        agent.Value.velocity = Vector3.zero;
        agent.Value.isStopped = true;
        return TaskStatus.Success;
    }


}
