using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollectableWeaponStruct
{
    // Basic Item Struct
    public string itemName;
    public SpriteData itemImage;
    public int quantity;
    public string itemType;

    // Damage structure
    public int[] damageAmount;
    public float[] pushForce;

    // Weapon Upgrade
    public int weaponLevel;
}

