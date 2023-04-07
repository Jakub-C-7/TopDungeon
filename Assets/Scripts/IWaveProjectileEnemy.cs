using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaveProjectileEnemy : IProjectileEnemy
{
    public int GetRound();
    public void SetRound(int round);
    public int GetMaxRound();

}
