using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{

    [Header(" Data ")]
    [SerializeField] private CharacterDataSO playerData;

    [Header(" Settings ")]
    //private Dictionary<Stat, float> statDatas = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();

    private void Awake()
    {
        playerStats = playerData.BaseStats;

        foreach (KeyValuePair<Stat, float> kvp in playerStats)
            addends.Add(kvp.Key, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayerStat(Stat stat, float value)
    {
        // player  ->  base stats

        // addends -> upgrades in the wave transition
        // Stat - value
        if (addends.ContainsKey(stat))
            addends[stat] += value;
        else
            Debug.LogError($"The key {stat} has not been found in the dictionary");

        UpdatePlayerStats();
        // objects -> list object stats
    }

    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];

        return value;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies = 
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IPlayerStatsDependency>();

        foreach (IPlayerStatsDependency dependency in playerStatsDependencies)
            dependency.UpdateStats(this);
    }
}
