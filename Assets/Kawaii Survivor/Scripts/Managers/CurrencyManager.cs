using NaughtyAttributes;
using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }


    [Header(" Actions ")]
    public static Action onCurrencyChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => UpdateTexts();

    [Button]
    private void Add500Currency() => AddCurrency(500);

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();

        onCurrencyChanged?.Invoke();
    }
    public void UseCurrency(int price) => AddCurrency(-price);

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());
    }

    public bool HasEnoughCurrency(int price) => Currency >= price;
}
