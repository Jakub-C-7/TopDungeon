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
        PopulateEquippedInventory();
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
                LoopOverWeaponList(GameManager.instance.player.inventory.weaponGearInventoryContents);
                break;

            case "Armour":
                LoopOverArmourList(GameManager.instance.player.inventory.armourGearInventoryContents);
                break;

            case "Consumable":
                LoopOverList(GameManager.instance.player.inventory.consumableInventoryContents);
                break;

            case "Resource":
                LoopOverList(GameManager.instance.player.inventory.resourceInventoryContents);
                break;

        }

    }


    public void PopulateEquippedInventory()
    {
        //References
        GameObject inventoryMenu = GameObject.Find("InventoryMenu");
        GameObject characterPanel = GameObject.Find("CharacterPanel");
        GameObject equippedInventory = GameObject.Find("EquippedInventory");
        string itemSlot;

        //Loading equipped inventory details
        foreach (CollectableItem i in equippedInventory.GetComponentsInChildren<CollectableItem>())
        {

            switch (i.itemType)
            {
                case "Weapon":
                    itemSlot = "WeaponHolster";
                    break;
                case "Armour":
                    itemSlot = "ArmourHolster";
                    break;
                case "Consumable":
                    itemSlot = "ConsumableHolsterOne";
                    Debug.Log("Consumable PopulateEquippedInventory not yet implemented");
                    break;
                default:
                    itemSlot = "empty";
                    break;
            }

            GameObject gearSlot = characterPanel.transform.Find(itemSlot).gameObject; // Get the gear slot to add an item to

            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object

            objToSpawn.transform.SetParent(gearSlot.transform); // Transfer ownership of object

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

            if (i.gameObject.GetComponent<CollectableWeapon>())
            {
                // Create CollectableWeapon instance and assign its data
                objToSpawn.AddComponent<CollectableWeapon>();
                objToSpawn.GetComponent<CollectableWeapon>().itemName = i.gameObject.GetComponent<CollectableWeapon>().itemName;
                objToSpawn.GetComponent<CollectableWeapon>().itemType = i.gameObject.GetComponent<CollectableWeapon>().itemType;
                objToSpawn.GetComponent<CollectableWeapon>().quantity = i.gameObject.GetComponent<CollectableWeapon>().quantity;
                objToSpawn.GetComponent<CollectableWeapon>().itemImage = i.gameObject.GetComponent<CollectableWeapon>().itemImage;
                objToSpawn.GetComponent<CollectableWeapon>().damageAmount = i.gameObject.GetComponent<CollectableWeapon>().damageAmount;
                objToSpawn.GetComponent<CollectableWeapon>().pushForce = i.gameObject.GetComponent<CollectableWeapon>().pushForce;
                objToSpawn.GetComponent<CollectableWeapon>().weaponLevel = i.gameObject.GetComponent<CollectableWeapon>().weaponLevel;

            }
            else
            {
                // Create CollectableItem instance and assign its data
                objToSpawn.AddComponent<CollectableItem>();
                objToSpawn.GetComponent<CollectableItem>().itemName = i.itemName;
                objToSpawn.GetComponent<CollectableItem>().itemType = i.itemType;
                objToSpawn.GetComponent<CollectableItem>().quantity = i.quantity;
                objToSpawn.GetComponent<CollectableItem>().itemImage = i.itemImage;

            }


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

    private void LoopOverWeaponList(List<CollectableWeapon> playerInventory)
    {
        GameObject inventoryMenu = GameObject.Find("InventoryMenu");
        GameObject bagPanel = GameObject.Find("BagPanel");

        //Loading inventory details
        foreach (CollectableWeapon i in playerInventory)
        {

            int currentIndex = playerInventory.IndexOf(i); // Get slot of item

            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object

            objToSpawn.transform.parent = bagPanel.transform.GetChild(currentIndex); // Transfer ownership of object

            // Create image and assign its data
            objToSpawn.AddComponent<Image>().sprite = i.itemImage;

            // Create CollectableWeapon instance and assign its data
            objToSpawn.AddComponent<CollectableWeapon>();
            objToSpawn.GetComponent<CollectableWeapon>().itemName = i.gameObject.GetComponent<CollectableWeapon>().itemName;
            objToSpawn.GetComponent<CollectableWeapon>().itemType = i.gameObject.GetComponent<CollectableWeapon>().itemType;
            objToSpawn.GetComponent<CollectableWeapon>().quantity = i.gameObject.GetComponent<CollectableWeapon>().quantity;
            objToSpawn.GetComponent<CollectableWeapon>().itemImage = i.gameObject.GetComponent<CollectableWeapon>().itemImage;
            objToSpawn.GetComponent<CollectableWeapon>().damageAmount = i.gameObject.GetComponent<CollectableWeapon>().damageAmount;
            objToSpawn.GetComponent<CollectableWeapon>().pushForce = i.gameObject.GetComponent<CollectableWeapon>().pushForce;
            objToSpawn.GetComponent<CollectableWeapon>().weaponLevel = i.gameObject.GetComponent<CollectableWeapon>().weaponLevel;

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

    private void LoopOverArmourList(List<CollectableArmour> playerInventory)
    {
        GameObject inventoryMenu = GameObject.Find("InventoryMenu");
        GameObject bagPanel = GameObject.Find("BagPanel");

        //Loading inventory details
        foreach (CollectableArmour i in playerInventory)
        {

            int currentIndex = playerInventory.IndexOf(i); // Get slot of item

            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object

            objToSpawn.transform.parent = bagPanel.transform.GetChild(currentIndex); // Transfer ownership of object

            // Create image and assign its data
            objToSpawn.AddComponent<Image>().sprite = i.itemImage;

            // Create CollectableWeapon instance and assign its data
            objToSpawn.AddComponent<CollectableArmour>();
            objToSpawn.GetComponent<CollectableArmour>().itemName = i.gameObject.GetComponent<CollectableArmour>().itemName;
            objToSpawn.GetComponent<CollectableArmour>().itemType = i.gameObject.GetComponent<CollectableArmour>().itemType;
            objToSpawn.GetComponent<CollectableArmour>().quantity = i.gameObject.GetComponent<CollectableArmour>().quantity;
            objToSpawn.GetComponent<CollectableArmour>().itemImage = i.gameObject.GetComponent<CollectableArmour>().itemImage;
            objToSpawn.GetComponent<CollectableArmour>().protectionAmount = i.gameObject.GetComponent<CollectableArmour>().protectionAmount;
            objToSpawn.GetComponent<CollectableArmour>().specialEffect = i.gameObject.GetComponent<CollectableArmour>().specialEffect;
            objToSpawn.GetComponent<CollectableArmour>().armourLevel = i.gameObject.GetComponent<CollectableArmour>().armourLevel;

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

        foreach (Transform i in GameObject.Find("CharacterPanel").transform)
        {

            // There should only be one item in each inventory slot
            if (i.childCount == 1)
            {
                //Destroy the one item within the slot.
                GameObject.Destroy(i.GetChild(0).gameObject);

            }

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
