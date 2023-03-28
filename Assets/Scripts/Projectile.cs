using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Collidable
{
    // Start is called before the first frame update
    public int damageAmount = 5;
    public float pushForce = 0.5f;
    public string target = "Enemy";
    public string origin;
    
    protected override void OnCollide(Collider2D coll)
    {
        if(target == "Player" && coll.name == "Player"){
            Damage(coll);
            
        }else if(target !="Player" && coll.tag == "Fighter" && coll.name !="Player"){
            Damage(coll);
        }
      
        if ( (coll.name != origin) && coll.name != "HitBox" && (transform.name != coll.name))
        {
            Destroy(this.gameObject);

        }
        
    }

    private void Damage(Collider2D coll){
         Damage damage = new Damage
            {
                damageAmount = damageAmount,
                origin = transform.position,
                pushForce = pushForce

            };
            coll.SendMessage("ReceiveDamage", damage);


    }
    public void SetProjectileStats(int damageAmount, float pushForce, string origin, string target){
        this.damageAmount = damageAmount;
        this.pushForce = pushForce;
        this.origin = origin;
        this.target = target;
    }
}
