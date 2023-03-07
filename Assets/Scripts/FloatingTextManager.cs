using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update()
    {
        foreach (FloatingText floatingText in floatingTexts)
        {
            floatingText.UpdateFloatingText();
        }
    }

    //Show the floating text
    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.textContent.text = msg;
        floatingText.textContent.fontSize = fontSize;
        floatingText.textContent.color = color;
        //Transfer world space to screen space to be used in the UI
        floatingText.textContent.gameObject.transform.position = Camera.main.WorldToScreenPoint(position);
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();

    }

    //Get or create floating text
    private FloatingText GetFloatingText()
    {
        FloatingText floatingText = floatingTexts.Find(t => !t.active); //Find a text that isn't active

        //If text isnt active and its empty, create a new text
        if (floatingText == null)
        {
            floatingText = new FloatingText();
            floatingText.gameObject = Instantiate(textPrefab); // Creating a new game objext
            floatingText.gameObject.transform.SetParent(textContainer.transform);
            floatingText.textContent = floatingText.gameObject.GetComponent<Text>(); // Set the text content of the FloatingText 

            floatingTexts.Add(floatingText);
        }
        return floatingText;
    }
}
