using NaughtyAttributes;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{

    [Header(" Elements ")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;


    [Header(" Data ")]
    [SerializeField] WeaponDataSo[] starterWeapons;
    private WeaponDataSo selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null)
                    return;

                playerWeapons.AddWeapon(selectedWeapon, initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;

            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    [Button]
    private void Configure()
    {
        // clean our parent, no children
        containersParent.Clear();

        // generate weapon selection containers
        GenerateWeaponContainers();
    }

    private void GenerateWeaponContainers()
    {
        for (int i = 0; i < 3; i++)
        {
            WeaponSelectionContainer containerInstance = Instantiate(weaponContainerPrefab, containersParent);

            WeaponDataSo weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

            int level = Random.Range(0, 4);
            initialWeaponLevel = level;

            containerInstance.Configure(weaponData.Sprite, weaponData.Name, level);

            containerInstance.Button.onClick.RemoveAllListeners();
            containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(containerInstance, weaponData));
        }
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSo weaponData)
    {
        selectedWeapon = weaponData;

        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {

            if (container == containerInstance)
                container.Select();
            else
                container.Deselect();
        }
    }
}
