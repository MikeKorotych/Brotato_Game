using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour, IGameStateListener
{

    [Header(" Elements ")]
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;

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
        switch (gameState)
        {
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

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
        }
    }
}
