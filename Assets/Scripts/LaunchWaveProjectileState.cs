using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchWaveProjectileState : IState
{
    private float waveCoolDown = 0.1f;
    private float lastWave;
    public void Enter(StateMachine stateMachine, Enemy enemy)
    {
        lastWave = Time.time - waveCoolDown;
    }

    public void Execute(StateMachine stateMachine, Enemy enemy)
    {
        
        if(Time.time - enemy.lastAttack > enemy.attackCooldown ){
            if((Time.time - lastWave > waveCoolDown)){
                lastWave = Time.time;
                int numberOfProjectiles = CalculateNumberOfProjectiles(enemy.projectile, 0.1f * (float)(enemy.round + 1));
                //int numberOfProjectiles = CalculateNumberOfProjectiles(enemy.projectile, 0.6f);

                float gap = (float)1/(float)numberOfProjectiles;

                for(int i = 0; i <numberOfProjectiles; i++ ){
                    GameObject projectileInstance = GameObject.Instantiate(enemy.projectile, RandomCircle(gap * i, enemy.transform.position , (enemy.round + 1)/(float)10), Quaternion.identity); 
                    Projectile projectileComponent = projectileInstance.AddComponent<Projectile>();
                    projectileComponent.SetProjectileStats(enemy.damageAmount, enemy.pushForce, enemy.transform.name, "Player");
                    GameObject.Destroy(projectileInstance, enemy.range);
                }
                enemy.round ++;

                if(enemy.round >= enemy.maxround){
                    enemy.lastAttack = Time.time;
                    enemy.round = 0;
                    stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
                }
            }
     }else{
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
     }
    }

    public void Exit()
    {
    }

    private int CalculateNumberOfProjectiles(GameObject projectile, float radius){
        float circumference = 2 * Mathf.PI * radius;
        float numberOfProjectiles = circumference / projectile.GetComponent<SpriteRenderer>().bounds.size.x/2;
        return Mathf.FloorToInt(numberOfProjectiles);
    }

    private Vector3 RandomCircle(float value, Vector3 center, float radius){ 
    // create random angle between 0 to 360 degrees 
        var ang = value * 360; 
        var pos = new Vector3();
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad); 
        pos.z = center.z; 
        return pos;
    }

    
}

