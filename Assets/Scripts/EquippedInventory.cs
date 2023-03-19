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

    }

    public void EquipItem(CollectableItem collectableItem)
    {
        Debug.Log("Item is being equipped: " + collectableItem.name);

        // Reference to the item in the Player's 'Inventory'
        GameObject itemInInventory = GameManager.instance.player.inventory.transform.Find(collectableItem.name).gameObject;

        switch (collectableItem.itemType)
        {

            case "Weapon":

                // Set parent of the item from 'Inventory' to the 'EquippedInventory'
                itemInInventory.transform.SetParent(this.gameObject.transform);

                // Remove the item being equipped from it's list within the 'Inventory'
                GameManager.instance.player.inventory.weaponGearInventoryContents.Remove(GameManager.instance.player.inventory.weaponGearInventoryContents.Find(x => x.name == collectableItem.itemName));

                // this.weapon = collectableItem;
                RefreshEquippedItems();

                break;

            case "Armour":

                // Set parent of the item from 'Inventory' to the 'EquippedInventory'
                itemInInventory.transform.SetParent(this.gameObject.transform);

                // Remove the item being equipped from it's list within the 'Inventory'
                GameManager.instance.player.inventory.armourGearInventoryContents.Remove(GameManager.instance.player.inventory.armourGearInventoryContents.Find(x => x.name == collectableItem.itemName));

                // this.armour = collectableItem;
                RefreshEquippedItems();

                break;

                // case "Consumable": //TODO: Determine if consumable1 slot or 2 are being used

                //     // Set parent of the item from 'Inventory' to the 'EquippedInventory'
                //     itemInInventory.transform.SetParent(this.gameObject.transform);

                //     // Remove the item being equipped from it's list within the 'Inventory'
                //     GameManager.instance.player.inventory.consumableInventoryContents.Remove(GameManager.instance.player.inventory.consumableInventoryContents.Find(x => x.name == collectableItem.itemName));

                //     this.consumableOne = collectableItem;

                //     break;

        }

    }

    public void UnEquipItem(Transform itemSlot, CollectableItem currentlyEquipped)
    {

        Debug.Log("Item is being unequipped: " + currentlyEquipped.name + "From slot: " + itemSlot.name);

        // Un-equip the equipped item - return it back into inventory
        // CollectableItem equippedItem = itemSlot.GetChild(0).gameObject.GetComponent<CollectableItem>();

        switch (itemSlot.name)
        {
            case "WeaponHolster":
                // this.weapon = null;
                RefreshEquippedItems();
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);
                GameManager.instance.player.inventory.ReAddItem(currentlyEquipped);

                break;

            case "ArmourHolster":

                RefreshEquippedItems();
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);
                GameManager.instance.player.inventory.ReAddItem(currentlyEquipped);

                break;

            case "ConsumableOneHolster":

                RefreshEquippedItems();
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);
                GameManager.instance.player.inventory.ReAddItem(currentlyEquipped);

                break;

            case "ConsumableTwoHolster":

                RefreshEquippedItems();
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);
                GameManager.instance.player.inventory.ReAddItem(currentlyEquipped);

                break;

        }


    }

}
