using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Fighter
{
    public Animator animator;
    // Start is called before the first frame update



    protected override void Death()
    {
        if (animator)
        {
            animator.SetBool("Destructed", true);
            Destroy(GetComponent<CapsuleCollider2D>());
        }
        else
        {
            if (this.gameObject.transform.parent.gameObject.transform.parent.name == "CompositePropMainParent")
            {
                Destroy(this.gameObject.transform.parent.gameObject.transform.parent.gameObject);

            }
            else
            {
                Destroy(this.gameObject.transform.parent.gameObject);

            }
        }

    }
}
