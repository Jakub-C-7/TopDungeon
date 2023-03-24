using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(inventoryMenu);
            Destroy(adventurerDiary.gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    //Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    //References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform healthBar;
    public Animator deathMenuAnimator;
    public GameObject hud;
    public GameObject menu;
    public SpeechBanner speechBanner;
    public AdventurerDiary adventurerDiary;
    public GameObject inventoryMenu;
    public List<GameObject> prefabList;


    //Logic
    public int experience;
    public int currentCharacterSelection;

    //Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }
    public void SetSpeechBannerText(List<string> conversationList, List<string> responseList, Dictionary<int, List<int>> conversationToResponseVertices, Dictionary<int, List<int>> responseToConversationVertices, Vector3 position, string npcName, Sprite portrait)
    {

        speechBanner.SetText(conversationList, responseList, conversationToResponseVertices, responseToConversationVertices, position, npcName, portrait);
    }
    public bool GetSpeechBannerShowing()
    {
        return speechBanner.GetSpeechBannerShowing();
    }

    //Upgrade Weapon
    public bool TryUpgradeWeapon()
    {

        if (weaponPrices.Count <= weapon.weaponLevel) // Is the weapon max level?
        {
            return false;
        }


        if (player.inventory.coins >= weaponPrices[weapon.weaponLevel]) // Try to upgrade if enough coins
        {
            player.inventory.coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    //Health Bar
    public void OnHealthChange()
    {
        float ratio = (float)player.hitPoints / (float)player.maxHitpoints;
        healthBar.localScale = new Vector3(ratio, 1, 1);
    }

    //Experience System
    public int GetCurrentLevel()
    {
        int level = 0;
        int accumulatedExperience = 0;

        while (experience >= accumulatedExperience)
        {
            if (level == xpTable.Count) // Max Level reached
            {
                return level;
            }

            accumulatedExperience += xpTable[level];
            level++;

        }

        return level;
    }

    public int GetXpToLevel(int level) // Calculate XP required to next level
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp) //Give XP to player
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    public void OnLevelUp() //Grant bonuses on level up
    {
        player.OnLevelUp();
        OnHealthChange();
    }

    //Saving and Keeping Game State
    // public void SaveState()
    // {
    //     //Creating the 'saving' string containing details to save in state
    //     string saving = "";
    //     saving += "0" + "|";
    //     saving += coins.ToString() + "|";
    //     saving += experience.ToString() + "|";
    //     saving += weapon.weaponLevel.ToString() + "|";
    //     saving += currentCharacterSelection.ToString();

    //     PlayerPrefs.SetString("SaveState", saving);

    //     Debug.Log("saveState");

    // }

    // public void LoadState(Scene s, LoadSceneMode mode)
    // {
    //     SceneManager.sceneLoaded -= LoadState;

    //     if (!PlayerPrefs.HasKey("SaveState"))
    //     {
    //         return;
    //     }
    //     string[] data = PlayerPrefs.GetString("SaveState").Split('|');

    //     coins = int.Parse(data[1]);

    //     experience = int.Parse(data[2]);

    //     if (GetCurrentLevel() != 1)
    //     {
    //         player.SetLevel(GetCurrentLevel());

    //     }

    //     weapon.SetWeaponLevel(int.Parse(data[3]));

    //     //Record current character selection and choose correct sprite
    //     currentCharacterSelection = int.Parse(data[4]);
    //     player.SwapSprite(currentCharacterSelection);

    //     player.transform.position = GameObject.Find("SpawnPoint").transform.position;

    //     Debug.Log("Loading State");
    //     // Debug.Log("results" + coins + " / " + experience + " / " + currentCharacterSelection);

    // }

    public void SaveState()
    {
        Debug.Log("saving state");

        SaveSystem.SavePlayer(this.player);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        Debug.Log("loading state");
        SceneManager.sceneLoaded -= LoadState;

        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null) // If there was a save file to read
        {

            //Loading player details
            this.player.hitPoints = data.health;
            this.player.maxHitpoints = data.maxHitpoints;
            this.experience = data.experience;
            this.player.inventory.coins = data.coins;
            currentCharacterSelection = data.currentCharacterSelection;
            // weapon.weaponLevel = data.weaponLevel;

            // Loading inventory details
            LoopOverInventoryList(data.consumableInventoryContents, this.player.inventory.consumableInventoryContents);
            LoopOverInventoryList(data.resourceInventoryContents, this.player.inventory.resourceInventoryContents);
            LoopOverWeaponsInInventoryList(data.weaponGearInventoryContents, this.player.inventory.weaponGearInventoryContents);
            LoopOverArmourInInventoryList(data.armourGearInventoryContents, this.player.inventory.armourGearInventoryContents);

            this.player.inventory.consumableMaxCapacity = data.consumableMaxCapacity;
            this.player.inventory.resourceMaxCapacity = data.resourceMaxCapacity;
            this.player.inventory.weaponGearMaxCapacity = data.weaponGearMaxCapacity;
            this.player.inventory.armourGearMaxCapacity = data.armourGearMaxCapacity;

            // Loading currently equipped
            SetCurrentlyEquippedWeaponData(data.equippedWeapon);
            SetCurrentlyEquippedArmourData(data.equippedArmour);


            //TODO: Currently equipped Consumables!

            // Current Levels and Sprite selection 
            if (GetCurrentLevel() != 1)
            {
                player.SetLevel(GetCurrentLevel());

            }

            // weapon.SetWeaponLevel(data.weaponLevel); 
            player.SwapSprite(currentCharacterSelection);

            // player.ClearEquippedWeapon();
            player.RefreshEquippedWeapon();

        }

    }

    public void LoopOverInventoryList(List<CollectableItemStruct> inventoryList, List<CollectableItem> targetList)
    {
        //Loading inventory details
        foreach (CollectableItemStruct i in inventoryList)
        {
            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object
            objToSpawn.transform.parent = this.player.inventory.transform; // Transfer ownership of object
            CollectableItem item = objToSpawn.AddComponent<CollectableItem>();

            //Assign data to item
            item.itemName = i.itemName;
            item.quantity = i.quantity;
            item.itemType = i.itemType;
            item.itemImage = SpriteData.ToSprite(i.itemImage);

            // Add the item into the correct inventory list
            targetList.Add(item);

        }
    }

    public void LoopOverWeaponsInInventoryList(List<CollectableWeaponStruct> inventoryList, List<CollectableWeapon> targetList)
    {
        //Loading weapons in inventory details
        foreach (CollectableWeaponStruct i in inventoryList)
        {
            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object
            objToSpawn.transform.parent = this.player.inventory.transform; // Transfer ownership of object
            CollectableWeapon item = objToSpawn.AddComponent<CollectableWeapon>();

            //Assign data to item
            item.itemName = i.itemName;
            item.quantity = i.quantity;
            item.itemType = i.itemType;
            item.itemImage = SpriteData.ToSprite(i.itemImage);

            item.pushForce = i.pushForce;
            item.weaponLevel = i.weaponLevel;
            item.damageAmount = i.damageAmount;

            // Add the item into the correct inventory list
            targetList.Add(item);

        }
    }

    public void LoopOverArmourInInventoryList(List<CollectableArmourStruct> inventoryList, List<CollectableArmour> targetList)
    {
        //Loading weapons in inventory details
        foreach (CollectableArmourStruct i in inventoryList)
        {
            GameObject objToSpawn = new GameObject(i.itemName); // Spawn a new object
            objToSpawn.transform.parent = this.player.inventory.transform; // Transfer ownership of object
            CollectableArmour item = objToSpawn.AddComponent<CollectableArmour>();

            //Assign data to item
            item.itemName = i.itemName;
            item.quantity = i.quantity;
            item.itemType = i.itemType;
            item.itemImage = SpriteData.ToSprite(i.itemImage);

            item.protectionAmount = i.protectionAmount;
            item.specialEffect = i.specialEffect;
            item.armourLevel = i.armourLevel;

            // Add the item into the correct inventory list
            targetList.Add(item);

        }
    }

    private void SetCurrentlyEquippedWeaponData(CollectableWeaponStruct weaponStruct)
    {
        if (!default(CollectableWeaponStruct).Equals(weaponStruct))
        {

            GameObject objToSpawn = new GameObject(weaponStruct.itemName); // Spawn a new object
            objToSpawn.transform.parent = this.player.equippedInventory.transform; // Transfer ownership of object
            CollectableWeapon weaponItem = objToSpawn.AddComponent<CollectableWeapon>();

            weaponItem.itemName = weaponStruct.itemName;
            weaponItem.itemImage = SpriteData.ToSprite(weaponStruct.itemImage);
            weaponItem.quantity = weaponStruct.quantity;
            weaponItem.itemType = weaponStruct.itemType;

            weaponItem.weaponLevel = weaponStruct.weaponLevel;
            weaponItem.damageAmount = weaponStruct.damageAmount;
            weaponItem.pushForce = weaponStruct.pushForce;

            this.player.equippedInventory.weapon = weaponItem;
        }

    }

    private void SetCurrentlyEquippedArmourData(CollectableArmourStruct armourStruct)
    {
        if (!default(CollectableArmourStruct).Equals(armourStruct))
        {
            GameObject objToSpawn = new GameObject(armourStruct.itemName); // Spawn a new object
            objToSpawn.transform.parent = this.player.equippedInventory.transform; // Transfer ownership of object
            CollectableArmour armourItem = objToSpawn.AddComponent<CollectableArmour>();

            armourItem.itemName = armourStruct.itemName;
            armourItem.itemImage = SpriteData.ToSprite(armourStruct.itemImage);
            armourItem.quantity = armourStruct.quantity;
            armourItem.itemType = armourStruct.itemType;

            armourItem.armourLevel = armourStruct.armourLevel;
            armourItem.protectionAmount = armourStruct.protectionAmount;
            armourItem.specialEffect = armourStruct.specialEffect;

            this.player.equippedInventory.armour = armourItem;

        }
    }


    public void LoopOverSpriteList(List<SpriteData> spriteList, List<Sprite> targetList)
    {
        Debug.Log(spriteList);
        foreach (SpriteData i in spriteList)
        {
            targetList.Add(SpriteData.ToSprite(i));
        }
    }

    // On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
        player.canMove = true;

    }

    //Death menu and Respawn
    public void Respawn()
    {
        deathMenuAnimator.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    // Inventory and Item system
    public bool TryCollectItem(CollectableItem item)
    {
        bool spaceInInventory = player.inventory.TryAddItemToInventory(item); // Add the item to the correct inventory list

        if (spaceInInventory)
        {
            item.transform.parent = player.inventory.transform; //Transfer ownership of item to player's inventory
            item.name = item.itemName;

            return true;

        }
        else
        {
            return false;

        }

    }

    public bool TryCollectWeapon(CollectableWeapon item)
    {
        bool spaceInInventory = player.inventory.TryAddWeaponToInventory(item); // Add the item to the correct inventory list

        if (spaceInInventory)
        {
            item.transform.parent = player.inventory.transform; //Transfer ownership of item to player's inventory
            item.name = item.itemName;

            return true;

        }
        else
        {
            return false;

        }

    }

    public bool TryCollectArmour(CollectableArmour item)
    {
        bool spaceInInventory = player.inventory.TryAddArmourToInventory(item); // Add the item to the correct inventory list

        if (spaceInInventory)
        {
            item.transform.parent = player.inventory.transform; //Transfer ownership of item to player's inventory
            item.name = item.itemName;

            return true;

        }
        else
        {
            return false;

        }

    }

    public void RegisterDeath(Sprite sprite)
    {
        adventurerDiary.RegisterDeath(sprite);
    }



}
