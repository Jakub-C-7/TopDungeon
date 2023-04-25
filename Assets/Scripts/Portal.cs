using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames;
    public Animator portalAnimator;
    public string interactionPrompt = "E";
    public float interactionPromptDelay = 0.5f;
    protected float firstCollide;
    protected bool firstCollideSaved = false;

    protected override void Update()
    {
        base.Update();

        Vector3 distanceFromOriginator = GameManager.instance.player.transform.position - transform.position;

        //If player stops colliding with portal, clear last collide time
        if (distanceFromOriginator.magnitude > (0.16 * 1f) && firstCollideSaved == true)
        {
            firstCollideSaved = false;
            firstCollide = 0f;

        }
    }

    protected override void OnCollide(Collider2D coll)
    {

        if (coll.name == "Player")
        {
            //Save first collide time
            if (firstCollideSaved == false)
            {
                firstCollideSaved = true;
                firstCollide = Time.time;

            }

            //Show interaction prompt after a delay
            if (Time.time - interactionPromptDelay > firstCollide)
            {
                GameManager.instance.ShowText(interactionPrompt, 25, Color.white, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);

            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (portalAnimator)
                {
                    portalAnimator.SetTrigger("Open");
                }

                // If a dungeon key has been collected, reset by removing it from the player's inventory
                if (GameManager.instance.player.inventory.transform.Find("Dungeon Key"))
                {
                    Destroy(GameManager.instance.player.inventory.transform.Find("Dungeon Key").gameObject);
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
