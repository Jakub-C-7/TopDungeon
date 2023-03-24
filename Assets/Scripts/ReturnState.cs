using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IState
{
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
        enemy.healthBar.SetActive(false);
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
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
