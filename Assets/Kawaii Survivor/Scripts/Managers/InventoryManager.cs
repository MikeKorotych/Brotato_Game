using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{


    [Header(" Player Components ")]
    [SerializeField] private PlayerObjects playerObjects;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header(" Elements ")]
    [SerializeField] private Transform intventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedCallback;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedCallback;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }

    private void Configure()
    {
        intventoryItemsParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null) 
                continue;

            Weapon weapon = weapons[i];
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);

            inventoryItem.Configure(weapon, i, () => ShowItemInfo(inventoryItem));
        }


        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        foreach (var objectData in objectDatas)
        {
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);

            inventoryItem.Configure(objectData, () => ShowItemInfo(inventoryItem));
        }
    }

    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon, container.Index);
        else
            ShowObjectInfo(container.ObjectData);
    }

    private void ShowWeaponInfo(Weapon weapon, int index)
    {
        itemInfo.Configure(weapon);

        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleWeapon(index));

        shopManagerUI.ShowItemInfo();
    }

    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Configure(objectData);

        itemInfo.RecycleButton.onClick.RemoveAllListeners();
        itemInfo.RecycleButton.onClick.AddListener(() => RecycleObject(objectData));

        shopManagerUI.ShowItemInfo();
    }
    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);

        Configure();

        shopManagerUI.HideItemInfo();

        Debug.Log("--- Recycling weapon at index " + index + " ---");
    }

    private void RecycleObject(ObjectDataSO objectToRecycle)
    {
        // remove from playerObjects
        playerObjects.RecycleObject(objectToRecycle);

        // destroy inventory item container
        Configure();

        // close item info
        shopManagerUI.HideItemInfo();
    }

    private void ItemPurchasedCallback() => Configure();

    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Configure();
        itemInfo.Configure(mergedWeapon);
    }
}
