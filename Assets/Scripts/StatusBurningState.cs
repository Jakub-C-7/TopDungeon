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
                Debug.Log("Should call setcolour..");
                GameManager.instance.StartCoroutine(SetColour(mover));
            }
        }else{
            stateMachine.RemoveState(this);
        }
    }

    IEnumerator SetColour(Mover mover){
        Debug.Log("chaning colour");
        mover.GetComponent<SpriteRenderer>().material.color = new Color32(255,128,0, 255);
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Changing colour back");
        mover.GetComponent<SpriteRenderer>().material.color = Color.white;
    }


    public void Exit()
    {
        //Debug.Log("No longer burning");
    }

    public string GetId(){
        return "StatusBurningState";
    }

 
}
