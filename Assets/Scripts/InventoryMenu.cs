using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
   
    public Animator inventoryMenuAnimator;

    void Update()
    {
        //Toggle map on and off
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleBool("Showing");

        }
    }


    
 


  

    public void ToggleBool(string name)
    {
        inventoryMenuAnimator.SetBool(name, !inventoryMenuAnimator.GetBool(name));
    }


}
