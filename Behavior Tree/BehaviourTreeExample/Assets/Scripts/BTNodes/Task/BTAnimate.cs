using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTAnimate : BTBaseNode
{
    private Animator animator;
    private List<string> animation;

    private AnimatorClipInfo[] CurrentClipInfo;
    private float currentDuration = 0;
    public BTAnimate(Animator animator, params string[] animation)
    {
        this.animator = animator;

        this.animation = new List<string>();

        foreach(string anim in animation)
        {
            this.animation.Add(anim);
        }




    }

    public override TaskStatus Run()
    {
        if(animation.Count > 1)
        {
            CurrentClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);

            if (currentDuration < CurrentClipInfo.Length)
            {
                currentDuration += Time.fixedDeltaTime;
                animator.Play(animation[0]);
            }
            else
            {
                animator.Play(animation[1]);
            }
        }
        else
        {
            animator.Play(animation[0]);
        }

        currentDuration = 0;
        return TaskStatus.Success;
    }
}
