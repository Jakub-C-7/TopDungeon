using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text fields
    public Text levelText, hitPointText, coinText, upgradeCostText, xpText;

    //Logic
    public int currentCharacterSelection;
    public Image characterSelectionSprite, weaponSprite;
    public RectTransform xpBar;

    //References
    public Animator menuAnimator;

    private void Update()
    {
        //Toggle menu screen on and off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleBool("Showing");

        }

    }

    //Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            //If the selection went too far to one side
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
            {
                currentCharacterSelection = 0;
            }

            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            //If the selection went too far to one side
            if (currentCharacterSelection < 0)
            {
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;
            }

            OnSelectionChanged();

        }
    }

    //Selecting new character sprite
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
        GameManager.instance.currentCharacterSelection = currentCharacterSelection;
        UpdateMenu();
    }

    //Weapon Upgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    //Update character information
    public void UpdateMenu()
    {
        //Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];

        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count) //Already on the max weapon
        {
            upgradeCostText.text = "MAX";

        }
        else //Update the cost for the next weapon
        {
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        }

        //Meta
        hitPointText.text = GameManager.instance.player.hitPoints.ToString();
        coinText.text = GameManager.instance.player.inventory.coins.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //XP Bar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total xp points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int previousLvlXp = GameManager.instance.GetXpToLevel(currentLevel - 1);
            int currentLvlXp = GameManager.instance.GetXpToLevel(currentLevel);

            int diff = currentLvlXp - previousLvlXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLvlXp;

            float completionRatio = (float)currentXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currentXpIntoLevel.ToString() + " / " + diff;

        }

        // Character Selector
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[GameManager.instance.currentCharacterSelection];

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleBool(string name)
    {
        menuAnimator.SetBool(name, !menuAnimator.GetBool(name));
    }


}
