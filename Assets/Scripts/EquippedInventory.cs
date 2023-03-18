using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedInventory : MonoBehaviour
{
    public CollectableItem weapon;
    public CollectableItem armour;
    public CollectableItem consumableOne;
    public CollectableItem consumableTwo;

    // References
    public InventoryMenu inventoryMenu;

    private void Start()
    {
        // Update currently equipped items
        RefreshEquippedItems();
    }

    public void RefreshEquippedItems()
    {
        // Get currently equipped items from slots in the Inventory Menu
        weapon = inventoryMenu.getEquippedWeapon();
        armour = inventoryMenu.getEquippedArmour();
        consumableOne = inventoryMenu.getEquippedConsumableOne();
        consumableTwo = inventoryMenu.getEquippedConsumableTwo();

        // Search the overall inventory for each item being equipped, 
        // if the item is equipped, remove from inventory

    }

}
