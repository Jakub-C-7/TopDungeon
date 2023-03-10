using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    public int healingAmount = 1;

    public float healCooldown = 1.0f;
    private float lastHeal;
    public AudioSource onHealAudioSource;


    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }
        if (Time.time - lastHeal > healCooldown)
        {
            if(onHealAudioSource){
                onHealAudioSource.Play();
            }
            lastHeal = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
