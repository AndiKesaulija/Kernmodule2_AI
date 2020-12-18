using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSetTarget : BTBaseNode
{
    private VariableGameObject target;
    private VariableGameObject newTarget;
   public BTSetTarget(VariableGameObject target, VariableGameObject newTarget)
    {
        this.target = target;
        this.newTarget = newTarget;
    }

    public override TaskStatus Run()
    {
        if (target != newTarget)
        {
            target.Value = newTarget.Value;
        }

        return TaskStatus.Success;
    }
}
