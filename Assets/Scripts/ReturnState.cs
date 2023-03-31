using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IEnemyState
{
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(false);
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
          enemy.UpdateMotor(enemy.startingPosition - enemy.transform.position);
          if(enemy.startingPosition == enemy.transform.position ){
            stateMachine.ChangeState(new IdleState());
          }
    }

    public void Exit()
    {
        
    }

    // Start is called before the first frame update
  
}
