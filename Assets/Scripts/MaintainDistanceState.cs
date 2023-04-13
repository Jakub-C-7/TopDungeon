using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainDistanceState : IEnemyState
{
    Transform playerTransform;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(true);
        playerTransform = GameObject.Find("Player").transform;
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        float distanceFromPlayer = Vector3.Distance(playerTransform.position, enemy.transform.position);

        if (distanceFromPlayer > 0.7f)
        {
            enemy.UpdateMotor((playerTransform.position - enemy.transform.position).normalized); // Run towards the player
        }
        else if (distanceFromPlayer < 0.4f)
        {
            enemy.UpdateMotor((enemy.transform.position - playerTransform.position).normalized); // Run away from player
        }


        if (Vector3.Distance(playerTransform.position, enemy.startingPosition) > enemy.chaseLength)
        {
            stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Retreating]);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Fighting]);
        }
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update

}
