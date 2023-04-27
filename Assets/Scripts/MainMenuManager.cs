using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        //Save the player's state
        GameManager.instance.SaveState();
        //Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    }
}
