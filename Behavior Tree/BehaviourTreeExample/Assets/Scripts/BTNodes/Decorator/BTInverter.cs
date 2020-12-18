using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInverter : BTBaseNode
{
    private BTBaseNode node;
    public BTInverter(BTBaseNode node)
    {
        this.node = node;
    }
    public override TaskStatus Run()
    {
        var result = node.Run();
        if(result == TaskStatus.Running) { return TaskStatus.Running; }
        return result == TaskStatus.Success ? TaskStatus.Failed : TaskStatus.Success;
    }
}
