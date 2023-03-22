using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    // Damage
    public int damageAmount = 1;
    public float pushForce = 3;
    public float attackCooldown = 1.5f;
    public float lastAttack;
    public Enemy enemy;

    protected override void OnCollide(Collider2D coll)
    {
        if (((coll.tag == "Fighter" && coll.name == "Player") && Time.time - lastAttack > attackCooldown) && !enemy.staggered)
        {
            lastAttack = Time.time;

            // Create a new damage object, before sending it to the player
            Damage damage = new Damage
            {
                damageAmount = damageAmount,
                origin = transform.position,
                pushForce = pushForce

            };

            coll.SendMessage("ReceiveDamage", damage);
        }
    }
}
