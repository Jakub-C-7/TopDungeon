using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingState : IEnemyState
{
    private bool exploded = false;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        enemy.animator.SetBool("Exploding", true);
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
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
