using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IdleState : IEnemyState
{
    Transform playerTransform;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(false);
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, enemy.startingPosition) < enemy.triggerLength)
            {
               stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);

            }else{
                 enemy.UpdateMotor(enemy.startingPosition - enemy.transform.position);
            }

        }
        else // The player is out of range
        {
            enemy.UpdateMotor(enemy.startingPosition - enemy.transform.position);
         

        }

        //Check for overlaps

    }

    public void Exit()
    {
    }

    // Start is called before the first frame update
     
 
}
