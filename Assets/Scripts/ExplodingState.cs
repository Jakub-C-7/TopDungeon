using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingState : IState
{
    private bool exploded = false;
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
        enemy.animator.SetBool("Exploding", true);
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
    {
        if(enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("exploded") ){
            if(!exploded){
                enemy.explosionParticleSystem.Play(); 
                exploded = true;
            }else{
                if(!enemy.explosionParticleSystem){
                    GameManager.Destroy(enemy.gameObject);
                }
            }
        }


    }

    public void Exit()
    {
    }

    
}
