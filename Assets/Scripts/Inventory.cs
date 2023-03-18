using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int coins;
    public int consumableMaxCapacity = 10;
    public int resourceMaxCapacity = 10;
    public int weaponGearMaxCapacity = 10;
    public int armourGearMaxCapacity = 10;

    public List<CollectableItem> consumableInventoryContents;
    public List<CollectableItem> resourceInventoryContents;
    public List<CollectableItem> weaponGearInventoryContents;
    public List<CollectableItem> armourGearInventoryContents;

    public bool TryAddItemToInventory(CollectableItem item)
    {

        // Check which inventory the item needs to be added to based on type
        if (item.itemType == "Consumable" && consumableInventoryContents.Count < consumableMaxCapacity)
        {
            consumableInventoryContents.Add(item);
            GameManager.instance.ShowText(item.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;
        }
        else if (item.itemType == "Weapon" && weaponGearInventoryContents.Count < weaponGearMaxCapacity)
        {
            weaponGearInventoryContents.Add(item);
            GameManager.instance.ShowText(item.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;

        }
        else if (item.itemType == "Armour" && armourGearInventoryContents.Count < armourGearMaxCapacity)
        {
            armourGearInventoryContents.Add(item);
            GameManager.instance.ShowText(item.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;

        }
        else if (item.itemType == "Resource" && resourceInventoryContents.Count < resourceMaxCapacity)
        {
            resourceInventoryContents.Add(item);
            GameManager.instance.ShowText(item.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;

        }
        else
        {
            GameManager.instance.ShowText("Inventory Full!", 20, Color.red, transform.position, Vector3.up * 50, 2.0f);

            return false;
        }
    }



}
