using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    IEnemyState currentState;
    Enemy enemy;

    public Dictionary<EnemyStatePhases, IEnemyState> stateMapper;

    public EnemyStateMachine(Enemy enemy){
        this.enemy = enemy;
    }

    public void ChangeState(IEnemyState newState){
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
