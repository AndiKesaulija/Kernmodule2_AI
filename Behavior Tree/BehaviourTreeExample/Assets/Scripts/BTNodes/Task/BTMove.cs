using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BTMove : BTBaseNode
{
    private VariableGameObject goal;
    private VariableNavMeshAgent agent;
    private float speed;


    public BTMove(float speed, VariableNavMeshAgent agent, VariableGameObject goal)
    {
        this.speed = speed;
        this.goal = goal;
        this.agent = agent;
    }

    public override TaskStatus Run()
    {

        agent.Value.speed = speed;
        agent.Value.isStopped = false;
        agent.Value.destination = goal.Value.transform.position;
        return TaskStatus.Success;
    }
        
    
}
