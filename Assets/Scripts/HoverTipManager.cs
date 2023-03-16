using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;

    public static Action<string, Vector2> onMouseHover;
    public static Action onMouseLoseFocus;

    private void OnEnable()
    {
        onMouseHover += ShowTip;
        onMouseLoseFocus += HideTip;

    }

    private void OnDisable()
    {
        onMouseHover -= ShowTip;
        onMouseLoseFocus -= HideTip;
    }

    void Start()
    {
        HideTip();


    }

    private void ShowTip(String tip, Vector2 mousePosition)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePosition.x + 50, mousePosition.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);

    }

}
