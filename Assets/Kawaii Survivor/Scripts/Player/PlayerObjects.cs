using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    private PlayerStatsManager playerStatsManager;

    private void Awake() => playerStatsManager = GetComponent<PlayerStatsManager>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var objectData in Objects)
            playerStatsManager.AddObject(objectData.BaseStats);
    }

    public void AddObject(ObjectDataSO objectData)
    {
        Objects.Add(objectData);
        playerStatsManager.AddObject(objectData.BaseStats);
    }

    public void RecycleObject(ObjectDataSO objectData)
    {
        // Remove object from list
        Objects.Remove(objectData);

        // get the money from the currency manager
        CurrencyManager.instance.AddCurrency(objectData.RecyclePrice);

        // remove the object stats from player stats manager
        playerStatsManager.RemoveObjectStats(objectData.BaseStats);
    }
}
