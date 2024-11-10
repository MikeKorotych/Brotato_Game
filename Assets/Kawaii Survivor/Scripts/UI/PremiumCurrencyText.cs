using TMPro;
using UnityEngine;

public class PremiumCurrencyText : MonoBehaviour
{
    [Header(" Elements ")]
    private TextMeshProUGUI text;


    public void UpdateText(string currencyString)
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();

        text.text = currencyString;
    }
}
