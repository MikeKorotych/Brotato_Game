using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatContainer : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image statImage;
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI statValueText;
    
    public void Configure(Sprite icon, string statName, string statValue)
    {
        statImage.sprite = icon;
        statText.text = statName;
        statValueText.text = statValue;
    }

    internal float GetFontSize()
    {
        return statText.fontSize;
    }

    internal void SetFontSize(float fontSize)
    {
        statText.fontSizeMax = fontSize;
        statText.fontSize = fontSize;
        statValueText.fontSize = fontSize;
    }
}
