using System;
using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;

    [Header(" Elements ")]
    [SerializeField] private StatContainer statContainer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent )
    {
        List<StatContainer> statContainers = new List<StatContainer>(); 

        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer containerInstance = Instantiate(statContainer, parent);
            statContainers.Add(containerInstance);

            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            string statValue = kvp.Value.ToString("F1");
            //string statValue = kvp.Value.ToString();

            containerInstance.Configure(icon, statName, statValue);
        }

        LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeText(statContainers));
    }

    private void ResizeText(List<StatContainer> statContainers)
    {
        float minFontSize = 5000;

        for (int i = 0; i < statContainers.Count; i++)
        {
            StatContainer statContainer = statContainers[i];
            float fontSize = statContainer.GetFontSize();

            if (fontSize < minFontSize)
                minFontSize = fontSize;
        }

        // at this point we have the minimum font size
        // set this font size to all stat containers

        for (int i = 0; i < statContainers.Count; i++)
        {
            statContainers[i].SetFontSize(minFontSize);
        }
    }

    public static void GenerateStatContainers(Dictionary<Stat, float> statDictionary, Transform parent)
    {
        instance.GenerateContainers(statDictionary, parent);
    }
}
