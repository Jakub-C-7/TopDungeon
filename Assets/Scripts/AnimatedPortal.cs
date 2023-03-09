using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPortal : Portal
{
    public Animator doorAnimator;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            doorAnimator.SetTrigger("Open");
        }

        base.OnCollide(coll);
    }

}
