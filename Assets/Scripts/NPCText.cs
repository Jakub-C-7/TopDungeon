using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collidable
{
    public List<string> conversationList;
    public List<string> responseList;
    public string conversationToResponse;
    public string responseToConversation;

    public float cooldown = 4.0f;
    private float lastShout = -4.0f;
    public bool inConversation;
    public string npcName = "NPC";
    public Dictionary<string,string> conversationAndResponse;

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
                    //create a vertice map for the directed dialgoue graph
                    Dictionary<int,List<int>> conversationToResponseVertices = CreateVertices(conversationToResponse);
                    Dictionary<int,List<int>> responseToConversationVertices = CreateVertices(responseToConversation);

                    lastShout = Time.time;
                    GameManager.instance.SetSpeechBannerText(conversationList, responseList, conversationToResponseVertices,responseToConversationVertices, gameObject.transform.position, npcName, portrait);  
                    inConversation = true;
                    //GameManager.instance.ShowText(message, 20, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, cooldown);
                
                }

            }
        }
        if(!GameManager.instance.GetSpeechBannerShowing()){
            inConversation = false;
        }
        
    }


    private Dictionary<int, List<int>> CreateVertices(string stringToSplit){
        Dictionary<int, List<int>> tempDict = new Dictionary<int, List<int>>();
  
        string[] words = stringToSplit.Split(';');
        foreach (string i in words){
            if(i == ""){ //if there are no dialogue options
                return tempDict;
            }
            string[] vertices = i.Split(',');
            int from = int.Parse(vertices[0]);
            int to = int.Parse(vertices[1]);
            if(!tempDict.ContainsKey(from)){
                tempDict.Add(from,new List<int>());
            }
            tempDict[from].Add(to);
        }
        return tempDict;
    }
}
