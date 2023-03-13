using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeechBanner : MonoBehaviour
{
    public Text displayedText;
    public Text displayedName;
    public Image displayedPortrait;

    public bool inConversation;
    public Animator speechAnimator;
    public List<string> conversationList;
    public List<string> responseList;

    private int currentTextDisplayed;
    
    private float conversationRange = 5f;
    private Vector3 originatorPosition;
    private float cooldown = 0.5f;
    private float lastInteraction;

    private Dictionary<int, List<int>> conversationToResponseVertices;
    private Dictionary<int, List<int>> responseToConversationVertices;

    public bool playerTurn;


    public void ChangeText(){
        if(currentTextDisplayed  < conversationList.Count){
            displayedText.text = conversationList[currentTextDisplayed];
            if(conversationToResponseVertices.ContainsKey(currentTextDisplayed)){
                int count = 0;
                foreach(int response in conversationToResponseVertices[currentTextDisplayed]){
                    displayedText.text += "\n" + count.ToString() + ": " + responseList[response];
                    count+=1;
                    playerTurn = true;
                }
            }else{
                playerTurn = false;
            }
            
            currentTextDisplayed +=1;
        }else{
            speechAnimator.SetBool("showing", false);
            inConversation = false;
        }
    }

    public void SetText(List<string> conversationList,List<string> responseList, Dictionary<int, List<int>> conversationToResponseVertices,Dictionary<int, List<int>> responseToConversationVertices, Vector3 originatorPosition, string npcName, Sprite portrait ){
        this.conversationToResponseVertices = conversationToResponseVertices;
        this.responseToConversationVertices = responseToConversationVertices;
        this.responseList = responseList;
        displayedPortrait.sprite = portrait;
        displayedName.text = npcName;
        inConversation = true;
        this.conversationList = conversationList;
        currentTextDisplayed = 0;
        ChangeText();
        this.originatorPosition = originatorPosition;
        lastInteraction = Time.time;
        speechAnimator.SetBool("showing", true);
     }

     protected void Update(){
        if(inConversation && Time.time - lastInteraction > cooldown){
            Vector3 distanceFromOriginator =  GameManager.instance.player.transform.position - originatorPosition;
            if(distanceFromOriginator.magnitude > (0.16 * conversationRange) ){
                speechAnimator.SetBool("showing", false);
                inConversation = false;
            // }else if(Input.GetKeyDown(KeyCode.E)){
            //     ChangeText();
            // }
            }else if(Input.anyKeyDown){
                if(playerTurn == false){
                    if(Input.GetKeyDown(KeyCode.E)){
                        ChangeText();
                    }
                }else{
                   int key = GetPressedNumber();
                   if(!(key == -1)){

                        int nextConversationIndex = returnNextConversationIndex(key);
                        if(!(nextConversationIndex == -1)){
                            currentTextDisplayed = nextConversationIndex;
                            ChangeText();
                        }
                        
                   }
                }
            }
        }
    }

    private int GetPressedNumber() {
        for (int number = 0; number <= 9; number++) {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }
        return -1;
    }


    private int returnNextConversationIndex(int selectedNumber){
        //ensure number is a possible dialogue response (-1 because currentTextDisplayed has been moved on in changeText())
        if(selectedNumber > conversationToResponseVertices[currentTextDisplayed - 1].Count){
            return -1;
        }else{
            //convert the relative index of the response to the actual repsonse index in the map
            int responseValue = conversationToResponseVertices[currentTextDisplayed - 1][selectedNumber];
            //find what dialogue option that response points to in the map
            return responseToConversationVertices[responseValue][0];
        }

    }


    public bool GetSpeechBannerShowing(){
        return speechAnimator.GetBool("showing");
    }

   
}
