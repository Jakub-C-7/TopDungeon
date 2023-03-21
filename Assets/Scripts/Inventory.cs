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
    public List<CollectableWeapon> weaponGearInventoryContents;
    public List<CollectableArmour> armourGearInventoryContents;

    public bool TryAddItemToInventory(CollectableItem item)
    {

        // Check which inventory the item needs to be added to based on type
        if (item.itemType == "Consumable" && consumableInventoryContents.Count < consumableMaxCapacity)
        {
            consumableInventoryContents.Add(item);
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

    public bool TryAddWeaponToInventory(CollectableWeapon collectableWeapon)
    {

        if (collectableWeapon.itemType == "Weapon" && weaponGearInventoryContents.Count < weaponGearMaxCapacity)
        {
            weaponGearInventoryContents.Add(collectableWeapon);
            GameManager.instance.ShowText(collectableWeapon.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;

        }
        else
        {
            GameManager.instance.ShowText("Inventory Full!", 20, Color.red, transform.position, Vector3.up * 50, 2.0f);

            return false;
        }

    }

    public bool TryAddArmourToInventory(CollectableArmour collectableArmour)
    {

        if (collectableArmour.itemType == "Armour" && armourGearInventoryContents.Count < armourGearMaxCapacity)
        {
            armourGearInventoryContents.Add(collectableArmour);
            GameManager.instance.ShowText(collectableArmour.itemName + " acquired!", 20, Color.cyan, transform.position, Vector3.up * 50, 2.0f);

            return true;

        }
        else
        {
            GameManager.instance.ShowText("Inventory Full!", 20, Color.red, transform.position, Vector3.up * 50, 2.0f);

            return false;
        }

    }

    public void ReAddItem(CollectableItem item)
    {
        Debug.Log("This is the item in ReAddItem: " + item.name);

        // Check which inventory the item needs to be added to based on type
        if (item.itemType == "Consumable" && consumableInventoryContents.Count < consumableMaxCapacity)
        {
            consumableInventoryContents.Add(item);

        }
        else if (item.itemType == "Resource" && resourceInventoryContents.Count < resourceMaxCapacity)
        {
            resourceInventoryContents.Add(item);

        }

    }

    public void ReAddWeapon(CollectableWeapon item)
    {

        if (item.itemType == "Weapon" && weaponGearInventoryContents.Count < weaponGearMaxCapacity)
        {
            weaponGearInventoryContents.Add(item);

        }
    }

    public void ReAddArmour(CollectableArmour item)
    {

        if (item.itemType == "Armour" && armourGearInventoryContents.Count < armourGearMaxCapacity)
        {
            armourGearInventoryContents.Add(item);

        }
    }



}
