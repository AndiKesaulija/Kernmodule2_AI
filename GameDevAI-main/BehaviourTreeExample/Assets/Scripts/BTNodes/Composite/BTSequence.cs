using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTBaseNode
{
    private BTBaseNode[] nodes;
    private List<BTBaseNode> runningNodes;

    public BTSequence(params BTBaseNode[] inputNodes)
    {
        nodes = inputNodes;
        runningNodes = new List<BTBaseNode>();
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
                    return TaskStatus.Failed;
                case TaskStatus.Success:
                    continue;
                case TaskStatus.Running:
                    if (runningNodes.Contains(nodes[i]))
                    {
                        runningNodes.Add(nodes[i]);
                    }
                    return TaskStatus.Running;
            }
        }

        foreach (BTBaseNode node in runningNodes)
        {
            TaskStatus result = node.Run();
            switch (result)
            {
                case TaskStatus.Failed:
                    return TaskStatus.Failed;
                case TaskStatus.Success:
                    runningNodes.Remove(node);
                    continue;
                case TaskStatus.Running:
                    if (runningNodes.Contains(node))
                    {
                        runningNodes.Add(node);
                    }
                    return TaskStatus.Running;
            }
        }
        if(runningNodes.Count == 0)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }

}
