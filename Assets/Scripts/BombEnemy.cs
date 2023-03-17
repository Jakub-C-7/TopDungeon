using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : Enemy
{
    public ParticleSystem explosionParticleSystem;
    private bool exploded = false;





    new protected void FixedUpdate(){
    
        if(!animator.GetBool("Exploding")){
           base.FixedUpdate();
        };

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("BombSmallEnemy_exploded") ){
            if(!exploded){
                explosionParticleSystem.Play(); 
                exploded = true;
            }else{
                if(!explosionParticleSystem){
                    Destroy(gameObject);
                }
            }
        }
     
        
    }

    

}

    
    

