using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator 
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level / 4;

        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();

        foreach (KeyValuePair<Stat, float> kvp in weaponData.BaseStats)
        {
            if(weaponData.Prefab.GetType() != typeof(RangeWeapon) && kvp.Key == Stat.Range)
                calculatedStats.Add(kvp.Key, kvp.Value);
            else
                calculatedStats.Add(kvp.Key, kvp.Value * multiplier);
        }

        return calculatedStats;
    }

    public static int GetPurchasePrice(WeaponDataSO weaponData, int level)
    {
        return level > 0 ? (int)((level + 1) * .9f  * weaponData.PurchasePrice) : weaponData.PurchasePrice;
    }

    public static int GetRecyclePrice(WeaponDataSO weaponData, int level)
    {
        return (int)(GetPurchasePrice(weaponData, level) * .75f);
    }
}
