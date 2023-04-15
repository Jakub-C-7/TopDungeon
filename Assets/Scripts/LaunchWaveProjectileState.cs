using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchWaveProjectileState : IEnemyState
{
    private float waveCoolDown = 0.1f;
    private float lastWave;
    private IWaveProjectileEnemy waveProjectileEnemy;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        lastWave = Time.time - waveCoolDown;
        waveProjectileEnemy = enemy as IWaveProjectileEnemy;
        enemy.healthBar.SetActive(true);
        if (waveProjectileEnemy == null)
        {
            throw new System.Exception("Enemy assigned LaunchProjectileState does not implement IProjectileEnemy");
        }
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (waveProjectileEnemy != null)
        {
            if (Time.time - waveProjectileEnemy.GetLastAttack() > waveProjectileEnemy.GetAttackCooldown())
            {
                if ((Time.time - lastWave > waveCoolDown))
                {
                    lastWave = Time.time;
                    int numberOfProjectiles = CalculateNumberOfProjectiles(waveProjectileEnemy.GetProjectilePreFab(), 0.1f * (float)(waveProjectileEnemy.GetRound() + 1));
                    //int numberOfProjectiles = CalculateNumberOfProjectiles(enemy.projectile, 0.6f);

                    float gap = (float)1 / (float)numberOfProjectiles;

                    for (int i = 0; i < numberOfProjectiles; i++)
                    {
                        GameObject projectileInstance = GameObject.Instantiate(waveProjectileEnemy.GetProjectilePreFab(), RandomCircle(gap * i, enemy.transform.position, (waveProjectileEnemy.GetRound() + 1) / (float)10), Quaternion.identity);
                        Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
                        Damage dmg = waveProjectileEnemy.GetProjectileDamageObject();
                        projectileComponent.SetProjectileStats(dmg.damageAmount, dmg.pushForce, enemy.transform.name, "Player");
                        GameObject.Destroy(projectileInstance, waveProjectileEnemy.GetRange());
                    }
                    waveProjectileEnemy.SetRound(waveProjectileEnemy.GetRound() + 1);

                    if (waveProjectileEnemy.GetRound() >= waveProjectileEnemy.GetMaxRound())
                    {
                        waveProjectileEnemy.SetLastAttack(Time.time);
                        waveProjectileEnemy.SetRound(0);
                        stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
                    }
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
            }
        }
    }

    public void Exit(EnemyStateMachine stateMachine, Enemy enemy)
    {
    }

    private int CalculateNumberOfProjectiles(GameObject projectile, float radius)
    {
        float circumference = 2 * Mathf.PI * radius;
        float numberOfProjectiles = circumference / projectile.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        return Mathf.FloorToInt(numberOfProjectiles);
    }

    private Vector3 RandomCircle(float value, Vector3 center, float radius)
    {
        // create random angle between 0 to 360 degrees 
        var ang = value * 360;
        var pos = new Vector3();
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }


}

