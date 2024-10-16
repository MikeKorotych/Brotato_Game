using System.Collections.Generic;
using UnityEngine;

public class StatContainerManager : MonoBehaviour
{
    public static StatContainerManager instance;

    [Header(" Elements ")]
    [SerializeField] private StatContainer statContainer;
    [SerializeField] private StatContainer shopStatContainer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void GenerateContainers(Dictionary<Stat, float> statDictionary, Transform parent, bool isShop = false)
    {
        List<StatContainer> statContainers = new List<StatContainer>();

        foreach (KeyValuePair<Stat, float> kvp in statDictionary)
        {
            StatContainer containerInstance;

            if (!isShop)
                containerInstance = Instantiate(statContainer, parent);
            else
                containerInstance = Instantiate(shopStatContainer, parent);

            statContainers.Add(containerInstance);

            Sprite icon = ResourcesManager.GetStatIcon(kvp.Key);
            string statName = Enums.FormatStatName(kvp.Key);
            float statValue = kvp.Value;
            //string statValue = kvp.Value.ToString();

            containerInstance.Configure(icon, statName, statValue);
        }

        //LeanTween.delayedCall(Time.deltaTime * 2, () => ResizeText(statContainers));
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
