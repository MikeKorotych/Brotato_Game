using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionContainer : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [field: SerializeField] public Button Button { get; private set; }


    [Header(" Stats ")]
    [SerializeField] private Transform statContainersParent;
    [SerializeField] private StatContainer statContainersPrefab;

    [Header(" Color ")]
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Outline outline;


    public void Configure(WeaponDataSo weaponData, int level)
    {
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name;

        Color imageColor = ColorHolder.GetColor(level);
        //nameText.color = imageColor;

        if(outline.enabled)
            outline.effectColor = ColorHolder.GetOutlineColor(level);

        foreach (Image image in levelDependentImages)
            image.color = imageColor;

        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
    }

    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        StatContainerManager.GenerateStatContainers(calculatedStats, statContainersParent);
    }

    public void Select()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one * 1.1f, .2f).setEase(LeanTweenType.easeOutCubic);
    }

    public void Deselect()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.one, .2f);
    }
}
