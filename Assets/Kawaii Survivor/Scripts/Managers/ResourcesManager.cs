using UnityEngine;

public static class ResourcesManager
{
    const string statIconDataPath = "Data/Stat Icons";
    const string objectDatasPath = "Data/Objects/";
    const string weaponDatasPath = "Data/Weapons/";

    private static StatIcon[] statIcons;

    public static Sprite GetStatIcon(Stat stat)
    {
        if (statIcons == null)
        {
            StatIconDataSO data = Resources.Load<StatIconDataSO>(statIconDataPath);
            statIcons = data.StatIcons;
        }

        foreach (StatIcon statIcon in statIcons)
        {
            if (stat == statIcon.stat)
                return statIcon.icon;
        }

        Debug.LogError("No icon found for stat: " + stat);

        return null;
    }

    private static ObjectDataSO[] objectDatas;
    public static ObjectDataSO[] Objects
    {
        get
        {
            if (objectDatas == null)
                objectDatas = Resources.LoadAll<ObjectDataSO>(objectDatasPath);


            return objectDatas;
        }
        private set { }
    }

    public static ObjectDataSO GetRandomObject() => Objects[Random.Range(0, Objects.Length)];

    ////////////////////////////////////////////////////////

    private static WeaponDataSO[] weaponDatas;
    public static WeaponDataSO[] Weapons
    {
        get
        {
            if (weaponDatas == null)
                weaponDatas = Resources.LoadAll<WeaponDataSO>(weaponDatasPath);


            return weaponDatas;
        }
        private set { }
    }

    public static WeaponDataSO GetRandomWeapon() => Weapons[Random.Range(0, Weapons.Length)];
}
