using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPrioritySelector : BTBaseNode
{
    private BTBaseNode[] nodes;

    public BTPrioritySelector(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;

    }
    public override TaskStatus Run()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            TaskStatus result = nodes[i].Run();
            currNode = nodes[i];

            switch (result)
            {
                case TaskStatus.Failed:
                    continue;
                case TaskStatus.Success:
                    i = i - 1;
                    return TaskStatus.Success;
                case TaskStatus.Running:
                    return TaskStatus.Running;
            }

        }
        return TaskStatus.Failed;

    }

}
