using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public Animator inventoryMenuAnimator;

    public string currentlySelectedTab;

    void Start()
    {
        RefreshCoins();
        ResetAndRepopulate("Weapon");
        ChangeCurrentTab("Weapon");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            RefreshCoins();
            ToggleBool("Showing");

        }
    }

    public void ResetAndRepopulate(string itemType)
    {
        CleanUp();
        PopulateInventory(itemType);
    }

    public void ChangeCurrentTab(string tabToChangeTo)
    {
        currentlySelectedTab = tabToChangeTo;
    }

    // Calls the LoopOverList function to populate the inventory bag by taking in a string which declares the itemType
    public void PopulateInventory(string itemType)
    {
        Debug.Log("Populate Inventory has been called");
        switch (itemType)
        {
            case "Weapon":
                LoopOverList(GameManager.instance.player.inventory.weaponGearInventoryContents);
                break;

            case "Armour":
                LoopOverList(GameManager.instance.player.inventory.armourGearInventoryContents);
                break;

            case "Consumable":
                LoopOverList(GameManager.instance.player.inventory.consumableInventoryContents);
                break;

            case "Resource":
                LoopOverList(GameManager.instance.player.inventory.resourceInventoryContents);
                break;

        }

    }

    public void RefreshCoins()
    {

        Text currentCoins = GameObject.Find("CoinPanel").transform.GetChild(0).GetComponent<Text>();
        currentCoins.text = GameManager.instance.player.inventory.coins.ToString();

    }

    // Loops over a Player's inventory list of CollectableItems and populates the bag 
    private void LoopOverList(List<CollectableItem> playerInventory)
    {
        //References
        GameObject inventoryMenu = GameObject.Find("InventoryMenu");
        GameObject bagPanel = GameObject.Find("BagPanel");

        //Loading inventory details
        foreach (CollectableItem i in playerInventory)
        {
            int currentIndex = playerInventory.IndexOf(i); // Get slot of item

            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object

            objToSpawn.transform.parent = bagPanel.transform.GetChild(currentIndex); // Transfer ownership of object

            // Create image and assign its data
            objToSpawn.AddComponent<Image>().sprite = i.itemImage;

            // Create CollectableItem instance and assign its data
            objToSpawn.AddComponent<CollectableItem>();
            objToSpawn.GetComponent<CollectableItem>().itemName = i.itemName;
            objToSpawn.GetComponent<CollectableItem>().itemType = i.itemType;
            objToSpawn.GetComponent<CollectableItem>().quantity = i.quantity;
            objToSpawn.GetComponent<CollectableItem>().itemImage = i.itemImage;

            // Attach hovertip to item
            objToSpawn.AddComponent<HoverTip>().tipToShow = i.itemName + "\nType: " + i.itemType;

            // Enable Drag and drop
            objToSpawn.AddComponent<DragDrop>().canvas = inventoryMenu.GetComponent<Canvas>();
            objToSpawn.AddComponent<CanvasGroup>();


            //Scale the object to each slot
            objToSpawn.transform.localPosition = Vector3.zero;
            objToSpawn.transform.localScale = new Vector3(0.64f, 0.64f, 0);

        }
    }

    // Clean up the backpack by removing existing objects
    public void CleanUp()
    {
        foreach (Transform i in GameObject.Find("BagPanel").transform)
        {

            // There should only be one item in each inventory slot
            if (i.childCount == 1)
            {
                //Destroy the one item within the slot.
                GameObject.Destroy(i.GetChild(0).gameObject);

            }
            //Below else if can be used as a contingency if clean up of multiple collectableItems in one inventory slot is needed
            // else if (i.childCount > 0)
            // {
            //     for (int n = 0; n < i.childCount; n++)
            //     {
            //         GameObject.Destroy(i.GetChild(n).gameObject);

            //     }
            // }

        }
    }

    private void ToggleBool(string name)
    {
        inventoryMenuAnimator.SetBool(name, !inventoryMenuAnimator.GetBool(name));
    }

    // Retrieve Weapon slot item
    // public CollectableItem getEquippedWeapon()
    // {
    //     return GameObject.Find("WeaponHolster").transform.GetComponentInChildren<CollectableItem>();
    // }

    // // Retrieve Armour slot item
    // public CollectableItem getEquippedArmour()
    // {
    //     return GameObject.Find("ArmourHolster").transform.GetComponentInChildren<CollectableItem>();
    // }

    // // Retrieve Consumable 1 slot item
    // public CollectableItem getEquippedConsumableOne()
    // {
    //     return GameObject.Find("ConsumableHolsterOne").transform.GetComponentInChildren<CollectableItem>();
    // }

    // // Retrieve Consumable 2 slot item
    // public CollectableItem getEquippedConsumableTwo()
    // {
    //     return GameObject.Find("ConsumableHolsterTwo").transform.GetComponentInChildren<CollectableItem>();
    // }


}
