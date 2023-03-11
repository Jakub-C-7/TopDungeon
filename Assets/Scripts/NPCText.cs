using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidable
{
    public string message;
    public float cooldown = 4.0f;
    private float lastShout = -4.0f;
    public Animator speechAnimator;
    public bool inConversation;

    protected override void OnCollide(Collider2D coll)
    {
        
                
        if(!inConversation){
            if (Time.time - lastShout > cooldown)
            {
                // NPC is available to interact with
                GameManager.instance.ShowText("E", 25, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, 0);

                if (Input.GetKeyDown(KeyCode.E))
                {
                
                    lastShout = Time.time;
                    GameManager.instance.SetSpeechBannerText(message, gameObject.transform.position);
                    speechAnimator.SetBool("showing", true);
                    inConversation = true;
                    
                    
                    //GameManager.instance.ShowText(message, 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
                
                }

            }
        }
        if(!speechAnimator.GetBool("showing")){
            inConversation = false;
        }
        
    }
}
