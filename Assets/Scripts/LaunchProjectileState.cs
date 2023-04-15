using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectileState : IEnemyState
{
    Transform playerTransform;
    IProjectileEnemy projectileEnemy;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        projectileEnemy = enemy as IProjectileEnemy;
        playerTransform = GameObject.Find("Player").transform;
        enemy.healthBar.SetActive(true);
        if (projectileEnemy == null)
        {
            throw new System.Exception("Enemy assigned LaunchProjectileState does not implement IProjectileEnemy");
        }
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (projectileEnemy != null)
        {
            if (Time.time - projectileEnemy.GetLastAttack() > projectileEnemy.GetAttackCooldown())
            {
                projectileEnemy.SetLastAttack(Time.time);
                // GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
                GameObject projectileInstance = GameObject.Instantiate(projectileEnemy.GetProjectilePreFab(), enemy.transform.position, Quaternion.identity) as GameObject;
                // Projectile projectileComponent =  projectileInstance.AddComponent<Projectile>();
                Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
                //  projectileComponent.transform.position = enemy.transform.position;
                Damage dmg = projectileEnemy.GetProjectileDamageObject();
                projectileComponent.SetProjectileStats(dmg.damageAmount, dmg.pushForce, enemy.transform.name, "Player");
                projectileComponent.statusState = GameManager.instance.StatusTypeResolver[projectileComponent.statusEffect];
                projectileInstance.GetComponent<Rigidbody2D>().velocity = (playerTransform.position - enemy.transform.position).normalized;
                Vector3 velocity = projectileInstance.GetComponent<Rigidbody2D>().velocity;
                projectileInstance.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
                GameObject.Destroy(projectileInstance, projectileEnemy.GetRange());

            }

            stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
        }

    }

    public void Exit(EnemyStateMachine stateMachine, Enemy enemy)
    {
    }


}
