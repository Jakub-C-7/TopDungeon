using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSmallEnemy : Enemy
{

     new protected void FixedUpdate(){
    
        if(distanceToPlayer < 0.5f){
            Debug.Log("so close");
        }else{
            Debug.Log("yet so far");
        }
        
        stateMachine.Update();
        
        
     
        
    }
   
}
