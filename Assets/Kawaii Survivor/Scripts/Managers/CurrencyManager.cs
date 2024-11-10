using NaughtyAttributes;
using System;
using UnityEngine;
using Tabsil.Sijil;

public class CurrencyManager : MonoBehaviour, IWantToBeSaved
{
    public static CurrencyManager instance;

    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int PremiumCurrency { get; private set; }


    [Header(" Actions ")]
    public static Action onCurrencyChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //AddPremiunCurrency(PlayerPrefs.GetInt(nameof(PremiumCurrency), 100), save: false);

        Candy.onCollected += CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;
    }

    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
        Cash.onCollected += CashCollectedCallback;
    }

    public void Load()
    {
        if (Sijil.TryLoad(this, nameof(PremiumCurrency), out object premiumCurrency))
            AddPremiunCurrency((int)premiumCurrency, save: false);
        else
            AddPremiunCurrency(500, save: false);
    }

    public void Save()
    {
        Sijil.Save(this, nameof(PremiumCurrency), PremiumCurrency);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => UpdateTexts();

    [Button]
    private void Add500Currency() => AddCurrency(500);

    [Button]
    private void Add500PremiunCurrency() => AddPremiunCurrency(500);

    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();
    }
    public void AddPremiunCurrency(int amount, bool save = true)
    {
        PremiumCurrency += amount;
        UpdateVisuals();

        //PlayerPrefs.SetInt(nameof(PremiumCurrency), PremiumCurrency);
    }

    private void UpdateVisuals()
    {
        UpdateTexts();
        onCurrencyChanged?.Invoke();
        Save();
    }

    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());

        PremiumCurrencyText[] premiumCurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (PremiumCurrencyText text in premiumCurrencyTexts)
            text.UpdateText(PremiumCurrency.ToString());
    }
    public void UseCurrency(int price) => AddCurrency(-price);
    public void UsePremiunCurrency(int price) => AddPremiunCurrency(-price);

    public bool HasEnoughCurrency(int price) => Currency >= price;
    public bool HasEnoughPremiunCurrency(int price) => PremiumCurrency >= price;

    private void CandyCollectedCallback(Candy candy) => AddCurrency(1);

    private void CashCollectedCallback(Cash cash) => AddPremiunCurrency(1);

}
