using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerDiary : MonoBehaviour
{
    public List<GameObject> characterLorePages;
    public List<GameObject> recipeCollectionPages;
    public List<GameObject> recordsPages;
    public List<Sprite> defeatedEnemies;
    public List<GameObject> activeList;
    public GameObject activePage;
    public GameObject displayKilledEnemiesSpriteList;
    public GameObject killedEnemiesViewPort;

    private int currentPage; 
    public Button recipeCollectionButton,characterLoreButton, recordsButton; 

    public Text levelText, hitPointText, expText, nextExpText;
    //References
    public Animator adventurerDiaryAnimator;


    void Start(){
        recipeCollectionButton.onClick.AddListener(() => SetActiveTab(recipeCollectionPages));
        characterLoreButton.onClick.AddListener(() => SetActiveTab(characterLorePages));
        activeList = characterLorePages;
        defeatedEnemies = new List<Sprite>();
    }

    private void Update()
    {
        //Toggle menu screen on and off
        if (Input.GetKeyDown(KeyCode.B))
        {
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

    public void RegisterDeath(Sprite recentEnemy){
        defeatedEnemies.Add(recentEnemy);
        GameObject recentEnemyObject = new GameObject();
        Image im = recentEnemyObject.AddComponent<Image>();
        recentEnemyObject.transform.SetParent(displayKilledEnemiesSpriteList.transform);

        RectTransform recentEnemyObjectRect = recentEnemyObject.GetComponent<RectTransform>();
        recentEnemyObject.transform.localScale = new Vector3(0.5f, 0.5f, 0f );
        recentEnemyObjectRect.anchorMin = new Vector2(0,1);
        recentEnemyObjectRect.anchorMax = new Vector2(0,1);
        recentEnemyObjectRect.pivot = new Vector2(0,1);

        recentEnemyObject.transform.localPosition = CalculateLatestPositionDefeatedEnemy();
        
        //recentEnemyObject.transform.localPosition = Vector3.zero;
        im.sprite = recentEnemy;

    }

    private Vector3 CalculateLatestPositionDefeatedEnemy(){
        int maxPerRow = 14;
        float y = (float)System.Math.Floor((double)(defeatedEnemies.Count - 1) / (double)maxPerRow) * -60;
        //find x position by taking position in list (base 0), remainder in current row, * gap size
        float x = ((defeatedEnemies.Count - 1) % maxPerRow) * 15;
        //set height of view so we can scroll to see enemies.
        RectTransform killedEnemiesViewPortRect = killedEnemiesViewPort.GetComponent<RectTransform>();
        killedEnemiesViewPortRect.sizeDelta = new Vector2(killedEnemiesViewPortRect.rect.x, System.Math.Abs(y));
        return new Vector3(x,y,0);
        
    
    }

    public void SetDefeatedEnemies(List<Sprite> defeatedEnemies){
        foreach (Sprite defeatedEnemy in defeatedEnemies){
            RegisterDeath(defeatedEnemy);
        }

    }


}
