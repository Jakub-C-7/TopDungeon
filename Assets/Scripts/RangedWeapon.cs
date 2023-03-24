using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);

        if (coll.name != "Player" && coll.name != "HitBox")
        {
            Destroy(this.gameObject);

        }

    }

    protected override void Update()
    {
        base.Update();

    }


    protected override void AttackController()
    {

        GameObject projectile = GameManager.instance.prefabList.Find(x => x.name.Equals("arrow_01"));

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                lastAttack = Time.time;

                GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 2.0f);
                arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(0, 1.0f) * Mathf.Rad2Deg);
                Destroy(arrow, 0.5f);
            }


        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                lastAttack = Time.time;

                GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -2.0f);
                arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(0, -1.0f) * Mathf.Rad2Deg);
                Destroy(arrow, 0.5f);

            }

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                lastAttack = Time.time;

                GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(2.0f, 0.0f);
                arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(-1.0f, 0) * Mathf.Rad2Deg);
                Destroy(arrow, 0.5f);

            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastAttack > cooldown)
            {
                lastAttack = Time.time;

                GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, 0.0f);
                arrow.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(1.0f, 0) * Mathf.Rad2Deg);
                Destroy(arrow, 0.5f);

            }

        }

    }


}
