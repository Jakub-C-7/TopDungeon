using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidable
{
    public List<string> conversationList;
    public float cooldown = 4.0f;
    private float lastShout = -4.0f;
    public bool inConversation;
    public string npcName = "NPC";

    public Sprite portrait;
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
                    GameManager.instance.SetSpeechBannerText(conversationList, gameObject.transform.position, npcName, portrait);  
                    inConversation = true;
                    //GameManager.instance.ShowText(message, 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
                
                }

            }
        }
        if(!GameManager.instance.GetSpeechBannerShowing()){
            inConversation = false;
        }
        
    }
}
