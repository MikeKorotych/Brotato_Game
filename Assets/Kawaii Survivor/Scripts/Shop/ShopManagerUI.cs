using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class ShopManagerUI : MonoBehaviour
{


    [Header(" Player Stats Elements ")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private GameObject playerStatsClosePanel;

    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
    }

    void Update()
    {
        
    }
    private void ConfigurePlayerStatsPanel()
    {
        float width = Screen.width * 0.2875f;
        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);
    }

    [Button]
    public void ShowPlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.SetActive(true);
    }

    [Button]
    public void HidePlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(false);
        playerStatsClosePanel.SetActive(false);
    }
}
