using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatAllEnemiesPortal : Portal
{
    protected override void OnCollide(Collider2D coll)
    {
        bool enemyExists = GameManager.FindAnyObjectByType<Enemy>();

        if (!enemyExists)
        {
            base.OnCollide(coll);

        }
        else if (coll.name == "Player" && enemyExists)
        {
            GameManager.instance.ShowText("Cannot travel with enemies nearby!", 20, Color.red, transform.position + new Vector3(0, 0.08f, 0), Vector3.zero, 0);

        }
    }
}

