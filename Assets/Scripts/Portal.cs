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
            GameManager.instance.player.canMove = false;
            Invoke("Teleport", 0.5f);
        }
    }

    private void Teleport()
    {
        //Save the player's state
        GameManager.instance.SaveState();

        //Teleport the player
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
