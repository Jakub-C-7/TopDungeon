using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    IState currentState;
    Enemy enemy;

    public Dictionary<EnemyStatePhases, IState> stateMapper;

    public StateMachine(Enemy enemy){
        this.enemy = enemy;
    }

    public void ChangeState(IState newState){
        if(currentState !=null){
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this, enemy);
    }

    public void Update(){
        if (currentState != null){
            currentState.Execute(this, enemy);
        }
    }
}
