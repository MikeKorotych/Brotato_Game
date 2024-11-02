using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instance;


    [Header(" Elements ")]
    [SerializeField] private PlayerWeapons playerWeapons;


    [Header(" Settings ")]
    private List<Weapon> weponsToMerge = new List<Weapon>();


    [Header(" Actions ")]
    public static Action<Weapon> onMerge;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 3)
            return false;

        weponsToMerge.Clear();
        weponsToMerge.Add(weapon);

        Weapon[] weapons = playerWeapons.GetWeapons();

        foreach (Weapon playerWeapon in weapons)
        {
            if (playerWeapon == null)
                continue;

            if (playerWeapon == weapon)
                continue;

            if (playerWeapon.WeaponData.Name != weapon.WeaponData.Name)
                continue;

            if (playerWeapon.Level != weapon.Level)
                continue;

            weponsToMerge.Add(playerWeapon);

            return true;
        }

        return false;
    }

    public void Merge()
    {
        if (weponsToMerge.Count < 2)
        {
            Debug.LogError("Not enough weapons to merge!");
            return;
        }

        DestroyImmediate(weponsToMerge[1].gameObject);
        weponsToMerge[0].Upgrade();

        Weapon weapon = weponsToMerge[0];
        weponsToMerge.Clear();

        onMerge?.Invoke(weapon);
    }
}
