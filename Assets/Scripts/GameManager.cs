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




    //Logic
    public int coins;
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


        if (coins >= weaponPrices[weapon.weaponLevel]) // Try to upgrade if enough coins
        {
            coins -= weaponPrices[weapon.weaponLevel];
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
    public void SaveState()
    {
        //Creating the 'saving' string containing details to save in state
        string saving = "";
        saving += "0" + "|";
        saving += coins.ToString() + "|";
        saving += experience.ToString() + "|";
        saving += weapon.weaponLevel.ToString() + "|";
        saving += currentCharacterSelection.ToString();

        PlayerPrefs.SetString("SaveState", saving);

        Debug.Log("saveState");

        // Debug.Log(saving);

    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        coins = int.Parse(data[1]);

        experience = int.Parse(data[2]);

        if (GetCurrentLevel() != 1)
        {
            player.SetLevel(GetCurrentLevel());

        }

        weapon.SetWeaponLevel(int.Parse(data[3]));

        //Record current character selection and choose correct sprite
        currentCharacterSelection = int.Parse(data[4]);
        player.SwapSprite(currentCharacterSelection);

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;

        Debug.Log("Loading State");
        // Debug.Log("results" + coins + " / " + experience + " / " + currentCharacterSelection);

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
    public void CollectItem(CollectableItem item)
    {
        player.inventory.AddItemToInventory(item);
        // Debug.Log(player.inventory.);
    }



}
