using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerDiary : MonoBehaviour
{
    public List<GameObject> characterLorePages;
    public List<GameObject> recipeCollectionPages;
    public List<GameObject> recordsPages;


    public List<GameObject> activeList;

    public GameObject activePage;

    private int currentPage; 
    public Button recipeCollectionButton,characterLoreButton, recordsButton; 

    public Text levelText, hitPointText, expText, nextExpText;


    //Text fields

    //References
    public Animator adventurerDiaryAnimator;

    void Start(){
        recipeCollectionButton.onClick.AddListener(() => SetActiveTab(recipeCollectionPages));
        characterLoreButton.onClick.AddListener(() => SetActiveTab(characterLorePages));
        activeList = characterLorePages;
    }
    private void Update()
    {
        //Toggle menu screen on and off
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Adventurer book reporting for duty");
            ToggleBool("Showing");
            updateDiary();
        }
    }

    private void SetActiveTab(List<GameObject> activeList){
        this.activeList = activeList;
        currentPage = 0;
        SetActivePage();
    }

    public void updateDiary(){
        int experience = GameManager.instance.experience;
        int level = GameManager.instance.GetCurrentLevel();
        
        hitPointText.text = GameManager.instance.player.hitPoints.ToString() + "/" + GameManager.instance.player.maxHitpoints;
        levelText.text = level.ToString();
        expText.text = experience.ToString();
        nextExpText.text = (GameManager.instance.GetXpToLevel(level + 1) - experience).ToString();


    }

    public void SetActivePage(){
         activePage.SetActive(false);
         activePage = activeList[currentPage];
         activePage.SetActive(true);
    }

    public void ChangePage(bool next){
        if(next){
            if(currentPage + 1 < activeList.Count){
                currentPage++;
            }
        }else{
            if(currentPage > 0){
                currentPage--;
            }
        }
        SetActivePage();
    }
  
   

    public void ToggleBool(string name)
    {
        adventurerDiaryAnimator.SetBool(name, !adventurerDiaryAnimator.GetBool(name));
    }


}
