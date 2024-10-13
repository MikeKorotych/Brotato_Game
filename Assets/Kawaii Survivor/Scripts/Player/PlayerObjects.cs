using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatsManager))]
public class PlayerObjects : MonoBehaviour
{
    [field: SerializeField] public List<ObjectDataSO> Objects { get; private set; }
    [SerializeField] PlayerStatsManager playerStatsManager;

    private void Awake() => playerStatsManager = GetComponent<PlayerStatsManager>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var objectData in Objects)
            playerStatsManager.AddObject(objectData.BaseStats);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
