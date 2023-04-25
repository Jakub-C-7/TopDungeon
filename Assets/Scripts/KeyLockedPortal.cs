using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockedPortal : Portal
{
    bool keyAcquired = false;

    protected override void Update()
    {
        base.Update();

        keyAcquired = GameManager.instance.player.inventory.transform.Find("Dungeon Key") != null;
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
                if (this.gameObject.name == "LadderPortal" && !keyAcquired)
                {
                    GameManager.instance.ShowText("Item required: Dungeon Key", 25, Color.red, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);

                }
                else
                {
                    GameManager.instance.ShowText(interactionPrompt, 25, Color.white, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);
                }

            }

            if (Input.GetKeyDown(KeyCode.E) && keyAcquired)
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
}
