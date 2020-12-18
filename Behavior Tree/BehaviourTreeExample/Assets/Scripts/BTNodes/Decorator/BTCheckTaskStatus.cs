using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTCheckTaskStatus : BTBaseNode
{
    private BTBaseNode activeTask;
    private List<BTBaseNode> replyNode;
    public BTCheckTaskStatus(BTBaseNode activeTask, params BTBaseNode[] replyNode)
    {
        this.activeTask = activeTask;
        this.replyNode = new List<BTBaseNode>();

        foreach(BTBaseNode node in replyNode)
        {
            this.replyNode.Add(node);
        }
    }
    public override TaskStatus Run() 
    {
        if (activeTask.Run() == TaskStatus.Success)
        {
            foreach (BTBaseNode node in replyNode)
            {
                node.Run();
            }
            return TaskStatus.Success;
        }
        else
        {
            activeTask.Run();
            return TaskStatus.Success;
        }
       
    }
}
