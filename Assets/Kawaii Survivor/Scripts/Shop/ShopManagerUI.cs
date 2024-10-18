using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerUI : MonoBehaviour
{
    [Header(" Player Stats Elements ")]
    [SerializeField] private RectTransform playerStatsPanel;
    [SerializeField] private RectTransform playerStatsClosePanel;
    private Vector2 playerStatsOpenedPos;
    private Vector2 playerStatsClosedPos;

    [Header(" Inventory Elements ")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform inventoryClosePanel;
    private Vector2 inventoryOpenedPos;
    private Vector2 inventoryClosedPos;


    [Header(" Item Info Elements")]
    [SerializeField] private RectTransform itemInfoSlidePanel;
    private Vector2 itemInfoOpenedPos;
    private Vector2 itemInfoClosedPos;


    IEnumerator Start()
    {
        yield return null;

        ConfigurePlayerStatsPanel();
        ConfigureInventoryPanel();
        ConfigureItemInfoPanel();
    }

    private void ConfigureInventoryPanel()
    {
        //float width = Screen.width * 0.2875f;
        float width = inventoryPanel.rect.width; // Получение ширины панели

        inventoryPanel.offsetMin = inventoryPanel.offsetMin.With(x: -width);

        inventoryOpenedPos = inventoryPanel.anchoredPosition;
        inventoryClosedPos = inventoryOpenedPos + new Vector2(width, 0);

        inventoryPanel.anchoredPosition = inventoryClosedPos;

        HideInventory();
    }

    [Button]
    public void HideInventory()
    {
        LeanTween.cancel(inventoryPanel);

        LeanTween.alpha(inventoryClosePanel, 0f, .2f).setRecursive(false).setEase(LeanTweenType.easeOutQuart);
        LeanTween.move(inventoryPanel, inventoryClosedPos, 0.2f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() =>
        {
            inventoryPanel.gameObject.SetActive(false);
            inventoryClosePanel.gameObject.SetActive(false);
        });

        HideItemInfo();
    }

    [Button]
    public void ShowInventory()
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryClosePanel.gameObject.SetActive(true);
        LeanTween.cancel(inventoryPanel);
        LeanTween.move(inventoryPanel, inventoryOpenedPos, .2f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.alpha(inventoryClosePanel, 0.25f, .2f).setRecursive(false).setEase(LeanTweenType.easeOutQuart);
    }

    //////////////////////////////////////// PLAYER STATS PAENEL ///////////////////////////////////////
    private void ConfigurePlayerStatsPanel()
    {
        //float width = Screen.width * 0.2875f;
        float width = playerStatsPanel.rect.width; // Получение ширины панели

        playerStatsPanel.offsetMax = playerStatsPanel.offsetMax.With(x: width);

        playerStatsOpenedPos = playerStatsPanel.anchoredPosition;
        playerStatsClosedPos = playerStatsOpenedPos - new Vector2(width, 0);

        playerStatsPanel.anchoredPosition = playerStatsClosedPos;

        HidePlayerStats();
    }

    [Button]
    public void ShowPlayerStats()
    {
        playerStatsPanel.gameObject.SetActive(true);
        playerStatsClosePanel.gameObject.SetActive(true);

        LeanTween.cancel(playerStatsPanel);

        // Плавно поднимаем альфа-канал
        LeanTween.alpha(playerStatsClosePanel, .25f, 0.2f);
        LeanTween.move(playerStatsPanel, playerStatsOpenedPos, 0.2f).setEase(LeanTweenType.easeOutQuart);
    }

    [Button]
    public void HidePlayerStats()
    {
        LeanTween.cancel(playerStatsPanel);

        // Плавно убираем альфа-канал
        LeanTween.alpha(playerStatsClosePanel, 0f, 0.2f).setRecursive(false);
        LeanTween.move(playerStatsPanel, playerStatsClosedPos, 0.2f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() =>
        {
            playerStatsPanel.gameObject.SetActive(false);
            playerStatsClosePanel.gameObject.SetActive(false);
        });

    }

    private void ConfigureItemInfoPanel()
    {
        float height = Screen.height / (2 * itemInfoSlidePanel.lossyScale.y);
        itemInfoSlidePanel.offsetMax = itemInfoSlidePanel.offsetMax.With(y: height);

        itemInfoOpenedPos = itemInfoSlidePanel.anchoredPosition;
        itemInfoClosedPos = itemInfoOpenedPos + Vector2.down * height;

        itemInfoSlidePanel.anchoredPosition = itemInfoClosedPos;

        HideItemInfo();
    }

    [Button]
    public void ShowItemInfo()
    {
        itemInfoSlidePanel.gameObject.SetActive(true);
        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel.LeanMove((Vector3)itemInfoOpenedPos, .2f).setEase(LeanTweenType.easeOutQuart);
        itemInfoSlidePanel.LeanAlpha(1, .2f);
    }

    [Button]
    public void HideItemInfo()
    {
        itemInfoSlidePanel.LeanCancel();
        itemInfoSlidePanel.LeanAlpha(0, .2f);
        itemInfoSlidePanel.LeanMove((Vector3)itemInfoClosedPos, .2f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() => itemInfoSlidePanel.gameObject.SetActive(false));
    }
}
