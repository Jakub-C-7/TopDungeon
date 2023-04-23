using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Fighter
{
    public Animator animator;
    // Start is called before the first frame update



    protected override void Death()
    {
        animator.SetBool("Destructed", true);
        Destroy(GetComponent<CapsuleCollider2D>());

    }
}
