using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public int hitPoints = 10;
    public int maxHitpoints = 10;
    public float pushRecoverySpeed = 0.1f;
    public AudioSource onHitAudioSource;
    //Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;
    
    public ParticleSystem part;
    //Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage / Die
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitPoints -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            float AngleRad = Mathf. Atan2(pushDirection.y, pushDirection.x);
         
            float AngleDeg = (180 / Mathf. PI) * AngleRad;
            if(part){
                    var shape = part.shape;
                // shape.rotation =  new Vector3(xAngle,yAngle, 0f);
                    shape.rotation =  new Vector3(0,0, AngleDeg - 22.5f);
                    part.Play();
            }
            //onHitAudioSource.Play();
             AudioSource.PlayClipAtPoint(onHitAudioSource.clip,this.gameObject.transform.position);

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 30, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitPoints <= 0)
            {
                hitPoints = 0;
                Death();
            }
        }
    }

    protected virtual void Death()
    {

    }
}
