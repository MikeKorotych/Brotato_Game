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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        foreach (var weapon in weapons)
        {
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);
            Color containerColor = ColorHolder.GetColor(weapon.Level);
            inventoryItem.Configure(containerColor, weapon.WeaponData.Sprite);
        }


        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();

        foreach (var objectData in objectDatas)
        {
            InventoryItemContainer inventoryItem = Instantiate(inventoryItemContainer, intventoryItemsParent);
            Color containerColor = ColorHolder.GetColor(objectData.Rarity);
            inventoryItem.Configure(containerColor , objectData.Icon);
        }
    }
}
