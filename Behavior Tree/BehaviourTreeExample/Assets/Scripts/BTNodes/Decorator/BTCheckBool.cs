using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTCheckBool : BTBaseNode
{
    private VariableBool condition;
    private bool desierdCondition;

    
    public BTCheckBool(VariableBool condition, bool desierdCondition)
    {
        this.condition = condition;
        this.desierdCondition = desierdCondition;

    }

    public override TaskStatus Run()
    {
        if (condition.Value == desierdCondition)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failed;
        }


    }

    
}
