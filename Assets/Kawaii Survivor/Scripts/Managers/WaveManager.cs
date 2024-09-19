using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] private Wave[] wave;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}


[System.Serializable]
public struct WaveSegment
{
    public GameObject prefab;
}