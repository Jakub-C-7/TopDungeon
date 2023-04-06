using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileEnemy
{
    public Damage GetProjectileDamageObject();
    public GameObject GetProjectilePreFab();
    public float GetRange();
    public float GetLastAttack();
    public void SetLastAttack(float time);
    public float GetAttackCooldown();
}
