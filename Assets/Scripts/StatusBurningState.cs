using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusBurningState : IStatusState
{
    float cooldown = 1.5f;
    float lastBurn;

    float duration;

    Damage dmg;

    

    public void Enter(StatusStateMachine stateMachine, Mover mover, float duration)
    {
      //  Debug.Log("Entered Burning");
        this.duration = duration;
        this.lastBurn = Time.time - cooldown;
        dmg = new Damage{
            damageAmount = 1,
            pushForce = 0f, 
            origin = Vector3.zero
        };

    }

    public void Execute(StatusStateMachine stateMachine, Mover mover)
    {
      //  Debug.Log("Executing");
        if(Time.time < duration){
            if(Time.time > lastBurn + cooldown){
                mover.SendMessage("ReceiveDamage", dmg);
                lastBurn = Time.time;
                GameManager.instance.StartCoroutine(SetColour(mover));
            }
        }else{
            stateMachine.RemoveState(this);
        }
    }

    IEnumerator SetColour(Mover mover){
        mover.GetComponent<SpriteRenderer>().material.SetVector("_GlowColour", new Color(1,0.27f,0,0) * 0.2f);
        yield return new WaitForSeconds(0.2f);
        mover.GetComponent<SpriteRenderer>().material.SetVector("_GlowColour", Color.white * 0f);
    }


    public void Exit()
    {
        //Debug.Log("No longer burning");
    }

    public string GetId(){
        return "StatusBurningState";
    }

 
}
