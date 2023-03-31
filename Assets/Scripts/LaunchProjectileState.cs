using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectileState : IEnemyState
{
    Transform playerTransform;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(true);
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
       if(Time.time - enemy.lastAttack > enemy.attackCooldown){
           enemy.lastAttack = Time.time;
           // GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            
            GameObject projectileInstance = GameObject.Instantiate(enemy.projectile, enemy.transform.position, Quaternion.identity) as GameObject;
           // Projectile projectileComponent =  projectileInstance.AddComponent<Projectile>();
            Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
          //  projectileComponent.transform.position = enemy.transform.position;
            projectileComponent.SetProjectileStats(enemy.damageAmount, enemy.pushForce, enemy.transform.name, "Player");
            projectileComponent.statusState = GameManager.instance.StatusTypeResolver[projectileComponent.statusEffect];
            projectileInstance.GetComponent<Rigidbody2D>().velocity = (playerTransform.position - enemy.transform.position).normalized;
            Vector3 velocity = projectileInstance.GetComponent<Rigidbody2D>().velocity;
            projectileInstance.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
            GameObject.Destroy(projectileInstance, enemy.range);

        }
       
        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
        
    }

    public void Exit()
    {
    }

  
}
