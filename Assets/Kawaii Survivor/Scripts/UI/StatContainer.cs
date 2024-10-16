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

    public void Configure(Sprite icon, string statName, float statValue, bool useColor = false)
    {
        statImage.sprite = icon;
        statText.text = statName;

        if (useColor)
            ColorizeStatValueText(statValue);
        else
        {
            statValueText.color = Color.white;
            statValueText.text = statValue.ToString("F0");
            //statValueText.text = statValue.ToString("F1");
        }

    }

    private void ColorizeStatValueText(float statValue)
    {

        float sign = MathF.Sign(statValue);

        if (statValue == 0)
            sign = 0;

        float absStatValue = MathF.Abs(statValue);

        Color statValueTextColor = Color.white;
        if (sign > 0)
            statValueTextColor = Color.green;
        else if (sign < 0)
            statValueTextColor = Color.red;

        statValueText.color = statValueTextColor;
        statValueText.text = absStatValue.ToString("F1");
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
