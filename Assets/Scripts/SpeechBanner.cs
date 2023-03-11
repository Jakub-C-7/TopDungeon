using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeechBanner : MonoBehaviour
{
    public Text speechText;
    public bool inConversation;
    public Animator speechAnimator;
    
    private float conversationRange = 5f;
    private Vector3 originatorPosition;

     public void SetText(string text, Vector3 originatorPosition){
        inConversation = true;
        speechText.text = text;
        this.originatorPosition = originatorPosition;

     }

     protected void Update(){
        if(inConversation){
            Vector3 distanceFromOriginator =  GameManager.instance.player.transform.position - originatorPosition;
            Debug.Log(distanceFromOriginator.magnitude);
            if(Input.GetKeyDown(KeyCode.E) || distanceFromOriginator.magnitude > (0.16 * conversationRange) ){
                    speechAnimator.SetBool("showing", false);
                    inConversation = false;
            }
        }
     }

}
