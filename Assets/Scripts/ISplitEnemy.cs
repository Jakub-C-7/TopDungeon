using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplitEnemy 
{
    public int GetNumberOfSplits();
    public void SetNumberOfSplits(int numOfSplits);
    public GameObject GetSplitClonePrefab();


}
