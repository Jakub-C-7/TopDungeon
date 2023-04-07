using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames;
    public Animator portalAnimator;

    protected override void OnCollide(Collider2D coll)
    {

        if (coll.name == "Player")
        {
            GameManager.instance.ShowText("E", 25, Color.white, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);

            if (coll.name == "Player" && Input.GetKeyDown(KeyCode.E))
            {
                if (portalAnimator)
                {
                    portalAnimator.SetTrigger("Open");
                }

                GameManager.instance.player.SetReduceLight(true);

                GameManager.instance.player.canMove = false;

                Invoke("Teleport", 0.5f);
            }
        }

    }

    private void Teleport()
    {
        //Save the player's state
        GameManager.instance.SaveState();

        //Teleport the player
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        GameManager.instance.player.SetReduceLight(false);

    }

    // private IEnumerator InitiateRandomDungeonCreation()
    // {
    //     var loadingScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2, UnityEngine.SceneManagement.LoadSceneMode.Additive);

    //     yield return new WaitForEndOfFrame();

    //     var newScene = UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(2);
    //     List<GameObject> gameObjectList = new List<GameObject>();

    //     gameObjectList.AddRange(newScene.GetRootGameObjects());

    //     var simpleRandomWalk = gameObjectList.Find(x => x.name == "SimpleRandomWalkGenerator");

    //     simpleRandomWalk.GetComponent<SimpleRandomWalkDungeonGenerator>().GenerateDungeon();

    // }

}
