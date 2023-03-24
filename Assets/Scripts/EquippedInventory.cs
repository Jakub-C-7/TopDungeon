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
        // RefreshEquippedItems();
    }

    public void ReloadEquippedItem(CollectableItem itemToReload)
    {

        switch (itemToReload.itemType)
        {
            case "Weapon":
                weapon = itemToReload;

                break;

            case "Armour":
                armour = itemToReload;

                break;

            case "Consumable":
                //TODO: Determine if consumable1 slot or 2 are being used
                break;


        }
    }

    // Equip a new item into an equippable slot
    public void EquipItem(CollectableItem collectableItem)
    {

        // Reference to the item in the Player's 'Inventory'
        GameObject itemInInventory = GameManager.instance.player.inventory.transform.Find(collectableItem.name).gameObject;

        // Set parent of the item from 'Inventory' to the 'EquippedInventory'
        itemInInventory.transform.SetParent(this.gameObject.transform);

        switch (collectableItem.itemType)
        {

            case "Weapon":

                // Remove the item being equipped from it's list within the 'Inventory'
                GameManager.instance.player.inventory.weaponGearInventoryContents.Remove(GameManager.instance.player.inventory.weaponGearInventoryContents.Find(x => x.name == collectableItem.itemName));

                weapon = this.gameObject.transform.Find(collectableItem.gameObject.name).GetComponent<CollectableItem>();

                GameManager.instance.player.RefreshEquippedWeapon(); // Refresh currently visible weapon

                break;

            case "Armour":

                // Remove the item being equipped from it's list within the 'Inventory'
                GameManager.instance.player.inventory.armourGearInventoryContents.Remove(GameManager.instance.player.inventory.armourGearInventoryContents.Find(x => x.name == collectableItem.itemName));

                armour = this.gameObject.transform.Find(collectableItem.gameObject.name).GetComponent<CollectableItem>();

                break;

            case "Consumable":
                //TODO: Determine if consumable1 slot or 2 are being used
                break;


        }

    }

    // Remove an already equipped item and return it back into the Inventory
    public void UnEquipItem(Transform itemSlot, CollectableItem currentlyEquipped)
    {

        // Un-equip the equipped item - return it back into inventory
        switch (itemSlot.name)
        {
            case "WeaponHolster":

                GameManager.instance.player.inventory.ReAddWeapon(this.gameObject.transform.Find(currentlyEquipped.name).gameObject.GetComponent<CollectableWeapon>());
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);

                GameManager.instance.player.ClearEquippedWeapon();
                weapon = null;

                break;

            case "ArmourHolster":

                GameManager.instance.player.inventory.ReAddArmour(this.gameObject.transform.Find(currentlyEquipped.name).gameObject.GetComponent<CollectableArmour>());
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);

                armour = null;

                break;

            case "ConsumableOneHolster":

                GameManager.instance.player.inventory.ReAddItem(this.gameObject.transform.Find(currentlyEquipped.name).gameObject.GetComponent<CollectableItem>());
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);

                consumableOne = null;

                break;

            case "ConsumableTwoHolster":

                GameManager.instance.player.inventory.ReAddItem(this.gameObject.transform.Find(currentlyEquipped.name).gameObject.GetComponent<CollectableItem>());
                this.gameObject.transform.Find(currentlyEquipped.name).SetParent(GameManager.instance.player.inventory.transform);

                consumableTwo = null;

                break;

        }

    }

}
