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
            }
        }else{
            stateMachine.RemoveState(this);
        }

    }

    public void Exit()
    {
        //Debug.Log("No longer burning");
    }

    public string GetId(){
        return "StatusBurningState";
    }

 
}
