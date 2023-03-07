using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : Fighter
{
    protected override void Death()
    {
        Destroy(gameObject);

    }

}
