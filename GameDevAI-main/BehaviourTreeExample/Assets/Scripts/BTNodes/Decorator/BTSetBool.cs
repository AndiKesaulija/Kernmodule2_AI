using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSetBool : BTBaseNode
{
    private VariableBool contition;
    private bool desierdCondition;
    public BTSetBool(VariableBool contition, bool desierdCondition)
    {
        this.contition = contition;
        this.desierdCondition = desierdCondition;
    }

    public override TaskStatus Run()
    {
        if(contition.Value != desierdCondition)
        {
            contition.Value = desierdCondition;
        }
        return TaskStatus.Success;
    }
}
