using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainDistanceState : IState
{
    Transform playerTransform;
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(true);
        playerTransform = GameObject.Find("Player").transform;
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
    {
        float distanceFromPlayer = Vector3.Distance(playerTransform.position,enemy.transform.position);

        if (distanceFromPlayer > 0.7f){
          enemy.UpdateMotor((playerTransform.position - enemy.transform.position).normalized); // Run towards the player
        }else if(distanceFromPlayer < 0.4f){
            enemy.UpdateMotor((enemy.transform.position - playerTransform.position).normalized); // Run away from player
        }

    
        if (Vector3.Distance(playerTransform.position, enemy.startingPosition) > enemy.chaseLength)
        {
            stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Idle]);      
        }else{
            stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Fighting]); 
        }
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update

}
