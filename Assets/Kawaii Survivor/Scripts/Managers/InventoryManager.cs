using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameStateListener
{


    [Header(" Player Components ")]
    [SerializeField] private PlayerObjects playerObjects;
    [SerializeField] private PlayerWeapons playerWeapons;

    [Header(" Elements ")]
    [SerializeField] private Transform intventoryItemsParent;
    [SerializeField] private Transform pauseIntventoryItemsParent;
    [SerializeField] private Transform gameOverIntventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;
    [SerializeField] private InventoryItemInfo itemInfo;

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergedCallback;
        GameManager.onGamePaused += GamePausedCallback;
    }

    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergedCallback;
        GameManager.onGamePaused -= GamePausedCallback;
    }

    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();

        if (gameState == GameState.GAMEOVER)
            Configure();
    }

    private void Configure()
    {
        intventoryItemsParent.Clear();
        pauseIntventoryItemsParent.Clear();
        gameOverIntventoryItemsParent.Clear();

        Weapon[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i] == null) 
                continue;

            Weapon weapon = weapons[i];

            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);
            inventoryItem.Configure(weapon, i, () => ShowItemInfo(inventoryItem));

            InventoryItemContainer pauseInventoryItem = Instantiate(inventoryItemContainer, pauseIntventoryItemsParent);
            pauseInventoryItem.Configure(weapon, i, null);

            InventoryItemContainer gameOverinventoryItem = Instantiate(inventoryItemContainer, gameOverIntventoryItemsParent);
            gameOverinventoryItem.Configure(weapon, i, null);
        }


        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        foreach (var objectData in objectDatas)
        {
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);
            inventoryItem.Configure(objectData, () => ShowItemInfo(inventoryItem));

            InventoryItemContainer pauseInventoryItem = Instantiate(inventoryItemContainer, pauseIntventoryItemsParent);
            pauseInventoryItem.Configure(objectData, null);

            InventoryItemContainer gameOverinventoryItem = Instantiate(inventoryItemContainer, gameOverIntventoryItemsParent);
            gameOverinventoryItem.Configure(objectData, null);
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

    private void GamePausedCallback() => Configure();

    private void WeaponMergedCallback(Weapon mergedWeapon)
    {
        Configure();
        itemInfo.Configure(mergedWeapon);
    }
}
