using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyHitbox : Collidable
{
    // Start is called before the first frame update
   protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" && coll.name == "Player")
        {
           Animator animator= GetComponentInParent<Animator>();
           animator.SetBool("Exploding", true);
        }
    }
}
