using System;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{

    [Header(" Actions ")]
    public static Action onCollected;

    public void Collect(Player player)
    {
        onCollected?.Invoke();
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
