using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemyState : IEnemyState
{
    ISplitEnemy splitEnemy;
    public void Enter(EnemyStateMachine stateMachine, Enemy enemy)
    {
        splitEnemy = enemy as ISplitEnemy;
        enemy.healthBar.SetActive(true);
        if (splitEnemy == null)
        {
            throw new System.Exception("Enemy assigned SplitEnemyState does not implement ISplitEnemy");
        }
    }

    public void Execute(EnemyStateMachine stateMachine, Enemy enemy)
    {
        if (splitEnemy != null)
        {
            enemy.animator.SetBool("Splitting", true);
            if (enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("split"))
            {
                GameObject cloneInstance = GameObject.Instantiate(splitEnemy.GetSplitClonePrefab(), enemy.transform.position, Quaternion.identity);
                Enemy cloneComponent = cloneInstance.GetComponent<Enemy>();
                cloneInstance.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                cloneComponent.startingPosition = enemy.startingPosition + new Vector3(0.32f, 0, 0);
                ISplitEnemy cloneSplitEnemy = cloneComponent as ISplitEnemy;
                cloneSplitEnemy.SetNumberOfSplits(splitEnemy.GetNumberOfSplits() - 1);
                splitEnemy.SetNumberOfSplits(0);
                stateMachine.ChangeState(stateMachine.stateMapper[EnemyStatePhases.Pathing]);
                enemy.animator.SetBool("Splitting", false);
            }
        }
    }

    public void Exit(EnemyStateMachine stateMachine, Enemy enemy)
    {
    }


}
