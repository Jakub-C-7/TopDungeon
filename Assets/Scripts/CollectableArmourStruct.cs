using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CollectableArmourStruct
{
    // Basic Item Struct
    public string itemName;
    public SpriteData itemImage;
    public int quantity;
    public string itemType;

    // Armour Specific
    public int[] protectionAmount;
    public int armourLevel;
    public string specialEffect;
}
