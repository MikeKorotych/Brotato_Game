using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{


    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;

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
            case GameState.GAME:
                gamePanel.SetActive(true);

                menuPanel.SetActive(false);
                shopPanel.SetActive(false);
                break;

            case GameState.WAVETRANSITION:
                waveTransitionPanel.SetActive(true);

                gamePanel.SetActive(false);
                menuPanel.SetActive(false);
                shopPanel.SetActive(false);
                break;

            case GameState.SHOP:
                shopPanel.SetActive(true);

                gamePanel.SetActive(false);
                menuPanel.SetActive(false);
                waveTransitionPanel.SetActive(false);
                break;

            case GameState.MENU:
                menuPanel.SetActive(true);

                gamePanel.SetActive(false);
                shopPanel.SetActive(false);
                waveTransitionPanel.SetActive(false);
                break;
        }

    }
}
