using NaughtyAttributes.Test;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{

    public void AddWeapon(WeaponDataSo selectedWeapon, int weaponLevel)
    {

        Debug.Log("--- We've selected weapon: " + selectedWeapon.name + "with weapon level: " + weaponLevel + "---");
    }
}
