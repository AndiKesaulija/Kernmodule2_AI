using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTWait : BTBaseNode
{
    private float duration;
    private float currentDuration = 0;

    private AnimatorClipInfo[] CurrentClipInfo;
    private Animator animator;


    public BTWait(float duration)
    {
        this.duration = duration;
    }

    public BTWait(Animator animator)
    {
        CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);

        this.duration = CurrentClipInfo.Length;
    }

    public override TaskStatus Run()
    {
        currentDuration += Time.fixedDeltaTime;
        if(currentDuration < duration)
        {
            //Debug.Log("Waiting: " + (duration - currentDuration));
            return TaskStatus.Running;
        }

        currentDuration = 0;
        return TaskStatus.Success;
    }
}
