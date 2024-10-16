using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] public Button purchaseButton;
    [Header(" Stats ")]
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private StatContainer statContainersPrefab;

    [Header(" Color ")]
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Outline outline;

    [Header(" Lock elements ")]
    [SerializeField] Image lockImage;
    [SerializeField] Sprite lockedSprite, unlockedSprite;
    public bool IsLocked { get; private set; }


    [Header(" Purchasing ")]
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }
    private int weaponLevel;

    [Header(" Actions  ")]
    public static Action<ShopItemContainer, int> onPurchased;


    private void Awake()
    {
        CurrencyManager.onCurrencyChanged += CurrencyUpdatedCallback;
    }

    private void OnDestroy()
    {
        CurrencyManager.onCurrencyChanged -= CurrencyUpdatedCallback;
    }
    private void CurrencyUpdatedCallback()
    {
        int itemPrice;

        if(WeaponData != null)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        else
            itemPrice = ObjectData.Price;

        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice);
    }


    public void Configure(WeaponDataSO weaponData, int level)
    {
        WeaponData = weaponData;
        weaponLevel = level;

        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name;

        int price = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        priceText.text = price.ToString();

        Color imageColor = ColorHolder.GetColor(level);
        //nameText.color = imageColor;

        if (outline.enabled)
            outline.effectColor = ColorHolder.GetOutlineColor(level);

        foreach (Image image in levelDependentImages)
            image.color = imageColor;

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(price);
    }
    public void Configure(ObjectDataSO objectData)
    {
        ObjectData = objectData;

        icon.sprite = objectData.Icon;
        icon.SetNativeSize();
        //icon.gameObject.transform.eulerAngles = Vector3.zero;
        nameText.text = objectData.Name;

        int price = objectData.Price;
        priceText.text = price.ToString();

        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        //nameText.color = imageColor;

        if (outline.enabled)
            outline.effectColor = ColorHolder.GetOutlineColor(objectData.Rarity);

        foreach (Image image in levelDependentImages)
            image.color = imageColor;

        ConfigureStatContainers(objectData.BaseStats);

        purchaseButton.onClick.AddListener(Purchase);
        purchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(price);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> stats)
    {
        statContainersParent.Clear();
        StatContainerManager.instance.GenerateContainers(stats, statContainersParent, isShop: true);
    }
    private void Purchase()
    {
        onPurchased?.Invoke(this, weaponLevel);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }

    private void UpdateLockVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
}
