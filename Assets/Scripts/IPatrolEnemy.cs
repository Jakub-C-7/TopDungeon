using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolEnemy
{
    // Start is called before the first frame update
    public List<Vector3> GetPatrolPoints();
}
