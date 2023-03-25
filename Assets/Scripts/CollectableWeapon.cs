using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWeapon : CollectableItem
{
    public string weaponType;

    // Damage structure
    public int[] damageAmount;
    public float[] pushForce;

    // Weapon Upgrade
    public int weaponLevel;

}
