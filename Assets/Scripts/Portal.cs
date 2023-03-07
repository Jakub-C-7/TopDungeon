using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            //TODO: Trigger door animation
            // Animator animator = this.transform.gameObject.GetComponent<Animator>();
            // animator.runtimeAnimatorController = Resources.Load("Open Door Portal") as RuntimeAnimatorController;
            //Save the player's state
            GameManager.instance.SaveState();
            //Teleport the player
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            Thread.Sleep(500); // 0.5 second delay
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
