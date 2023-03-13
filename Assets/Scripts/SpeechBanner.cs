using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeechBanner : MonoBehaviour
{
    public Text displayedText;
    public bool inConversation;
    public Animator speechAnimator;
    public List<string> conversationList;
    private int currentTextDisplayed;
    
    private float conversationRange = 5f;
    private Vector3 originatorPosition;
    private float cooldown = 0.5f;
    private float lastInteraction;


    public void ChangeText(){
        if(currentTextDisplayed + 1 <= conversationList.Count){
            displayedText.text = conversationList[currentTextDisplayed];
            currentTextDisplayed +=1;
        }else{
            speechAnimator.SetBool("showing", false);
            inConversation = false;
        }
    }

    public void SetText(List<string> conversationList, Vector3 originatorPosition){
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
            }else if(Input.GetKeyDown(KeyCode.E)){
                ChangeText();
            }
        }
    }

    public bool GetSpeechBannerShowing(){
        return speechAnimator.GetBool("showing");
    }
}
