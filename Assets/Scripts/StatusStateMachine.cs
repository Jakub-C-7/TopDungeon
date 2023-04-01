using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatusStateMachine
{
    Mover mover;
    public List<IStatusState> statusList;

    public StatusStateMachine(Mover mover){
        this.mover = mover;
        statusList = new List<IStatusState>();
    }

    public void AddState(IStatusState newState, float duration){
        System.Type newType = newState.GetType();

        foreach(IStatusState state in statusList){
            if (state.GetId().Equals(newState.GetId())){
                return;
            }
        }
        newState.Enter(this, mover, duration);

        statusList.Add(newState);
    }

    public void RemoveState(IStatusState stateToRemove){
        statusList.Remove(stateToRemove);
        stateToRemove.Exit();
    }

    public void RemoveAllStatusEffects(){
        statusList = new List<IStatusState>();
    }

    public void Update(){
        List<IStatusState> tempList = new List<IStatusState>(statusList);
        foreach(IStatusState state in tempList){
            state.Execute(this, mover);
        }
    }

    
}
