using NaughtyAttributes;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{

    [Header(" Elements ")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private PlayerWeapons playerWeapons;


    [Header(" Data ")]
    [SerializeField] WeaponDataSO[] starterWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;

    public void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null)
                    return;

                playerWeapons.TryAddWeapon(selectedWeapon, initialWeaponLevel);
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

            WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];

            int level = Random.Range(0, 4);

            containerInstance.Configure(weaponData, level);

            containerInstance.Button.onClick.RemoveAllListeners();
            containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(containerInstance, weaponData, level));
        }
    }

    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance, WeaponDataSO weaponData, int level)
    {
        selectedWeapon = weaponData;
        initialWeaponLevel = level;

        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {

            if (container == containerInstance)
                container.Select();
            else
                container.Deselect();
        }
    }
}
