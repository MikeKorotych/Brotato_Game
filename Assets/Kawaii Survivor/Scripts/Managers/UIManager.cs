using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{


    [Header(" Panels ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject restartConformaionPanel;

    private List<GameObject> panels = new List<GameObject>();

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            gameOverPanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel,
        });

        GameManager.onGamePaused += GamePausedCallback;
        GameManager.onGameResumed += GameResumedCallback;

        pausePanel.SetActive(false);
        HideRestartConformaionPanel();
    }

    private void OnDestroy()
    {
        GameManager.onGamePaused -= GamePausedCallback;
        GameManager.onGameResumed -= GameResumedCallback;
    }

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
                ShowPanel(gamePanel);
                break;

            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;

            case GameState.GAMEOVER:
                ShowPanel(gameOverPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;

            case GameState.MENU:
                ShowPanel(menuPanel);
                break;
        }


        // ���������� ���������� darkPanel
        darkPanel.SetActive(gameState != GameState.GAME);
    }

    private void ShowPanel(GameObject panel)
    {
        foreach (GameObject _panel in panels)
            _panel.SetActive(_panel == panel);
    }

    private void GamePausedCallback()
    {
        pausePanel.SetActive(true);
    }

    private void GameResumedCallback()
    {
        pausePanel.SetActive(false);
    }

    public void ShowRestartConformaionPanel()
    {
        restartConformaionPanel.SetActive(true);
    }
    public void HideRestartConformaionPanel()
    {
        restartConformaionPanel.SetActive(false);
    }
}
