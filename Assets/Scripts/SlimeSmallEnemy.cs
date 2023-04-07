using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSmallEnemy : Enemy, ISplitEnemy
{
    public int numOfSplits;
    GameObject clonePrefab;
    SlimeSmallEnemy(int numOfSplits){
        this.numOfSplits = numOfSplits;
    }

    protected override void Start()
    {
        
        base.Start();
        clonePrefab = GameManager.instance.prefabList.Find(x => x.name.Equals("SlimeSmallEnemy"));
          
        stateMachine.stateMapper = new Dictionary<EnemyStatePhases, IEnemyState>{
            [EnemyStatePhases.Idle] = new IdleState(),
            [EnemyStatePhases.Pathing] = new ChaseState()
       };
       
    }

    protected override void Execute(){
        if(hitPoints <= maxHitpoints/2 && numOfSplits > 0){
            // GameObject duplicateInstance = GameObject.Instantiate(creaturePrefab, transform.position, Quaternion.identity); 
            // SlimeSmallEnemy duplicateComponent = duplicateInstance.GetComponent<SlimeSmallEnemy>();
            // duplicateComponent.startingPosition = this.startingPosition + new Vector3 (0.32f, 0,0); 
            // duplicateComponent.numOfSplits = numOfSplits - 1;  
            // numOfSplits = 0;
            // Debug.Log("I have split");
            stateMachine.ChangeState(new SplitEnemyState());
        }
        stateMachine.Update(); 
    }

    public int GetNumberOfSplits()
    {
        return numOfSplits;
    }

    public void SetNumberOfSplits(int numOfSplits)
    {
        this.numOfSplits = numOfSplits;
    }

    public GameObject GetSplitClonePrefab()
    {
        return clonePrefab;
    }
}
