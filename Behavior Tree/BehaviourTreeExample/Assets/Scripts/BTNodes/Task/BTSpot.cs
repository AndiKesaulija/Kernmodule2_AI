using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSpot : BTBaseNode
{
    private VariableGameObject myAgent;
    private VariableGameObject target;
    private float spotDistance;
    private bool useAngle;

    private Vector3 eyeHeight = new Vector3(0, 1.5f, 0);
    public BTSpot(VariableGameObject target, VariableGameObject myAgent, float spotDistance, bool useAngle)
    {
        this.target = target;
        this.myAgent = myAgent;
        this.spotDistance = spotDistance;
        this.useAngle = useAngle;
    }
   
    public override TaskStatus Run()
    {
        Vector3 directionToTarget = myAgent.Value.transform.position - target.Value.transform.position;
        if(useAngle == true)
        {
            float angel = Vector3.Angle(myAgent.Value.transform.forward, directionToTarget);
            if (Mathf.Abs(angel) > 135)
            {
                RaycastHit hit;

                if (Physics.Raycast(myAgent.Value.transform.position + eyeHeight, -directionToTarget - eyeHeight, out hit, spotDistance))
                {
                    Debug.DrawRay(myAgent.Value.transform.position + eyeHeight, -directionToTarget - eyeHeight, Color.red);
                    if (hit.collider.tag == target.Value.tag)
                    {
                        return TaskStatus.Success;
                    }
                }

            }
            return TaskStatus.Failed;
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(myAgent.Value.transform.position + eyeHeight, -directionToTarget - eyeHeight, out hit, spotDistance))
            {
                Debug.DrawRay(myAgent.Value.transform.position + eyeHeight, -directionToTarget - eyeHeight, Color.red);
                if (hit.collider.tag == target.Value.tag)
                {
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failed;
        }

    }
}
