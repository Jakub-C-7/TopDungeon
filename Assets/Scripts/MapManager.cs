using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //References
    public Animator mapAnimator;


    // Update is called once per frame
    void Update()
    {
        //Toggle map on and off
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleBool("Showing");

        }
    }

    public void ToggleBool(string name)
    {
        mapAnimator.SetBool(name, !mapAnimator.GetBool(name));
    }
}
