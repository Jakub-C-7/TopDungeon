using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemCollidable : MonoBehaviour
{
    public int explosionDamageAmount;
    public float explosionPushForce;
    // Start is called before the first frame update
    void OnParticleCollision(GameObject other){
        if (other.name == "Player")
        {
            // Create a new damage object, before sending it to the player
            Damage damage = new Damage
            {
                damageAmount = explosionDamageAmount,
                origin = transform.position,
                pushForce = explosionPushForce

            };
            other.SendMessage("ReceiveDamage", damage);
        }
    }
}
