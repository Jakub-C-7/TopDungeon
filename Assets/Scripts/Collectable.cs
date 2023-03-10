using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collidable
{
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        if (!collected)
        {
            GameManager.instance.ShowText("E", 25, Color.white, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);

        }

        if (coll.name == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            onCollect();
        }
    }

    protected virtual void onCollect()
    {
        collected = true;
    }
}
