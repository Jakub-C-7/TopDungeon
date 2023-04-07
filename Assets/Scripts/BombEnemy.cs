using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{

    // new protected void FixedUpdate(){
    
    //     if(collidingWithPlayer){
    //         collidingWithPlayer = false;
    //         Debug.Log("Started exploding");
    //         stateMachine.ChangeState(new ExplodingState());
    //     }
    //     stateMachine.Update();   
    // }

    protected override void Execute(){
        if(collidingWithPlayer){
            collidingWithPlayer = false;
            Debug.Log("Started exploding");
            stateMachine.ChangeState(new ExplodingState());
        }
        stateMachine.Update(); 
    }

    

}

    
    

