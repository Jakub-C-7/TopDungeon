using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] projectileSpeeds = { 2.5f, -2.5f };
    public float distance = 0.25f;
    public Transform[] projectiles;


    private void Update()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            projectiles[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * projectileSpeeds[i]) * distance, Mathf.Sin(Time.time * projectileSpeeds[i]) * distance, 0);

        }

    }
}
