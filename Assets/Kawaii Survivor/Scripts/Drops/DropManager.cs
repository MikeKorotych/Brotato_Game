using System;
using UnityEngine;

public class DropManager : MonoBehaviour
{


    [Header(" Elements ")]
    [SerializeField] private Candy candyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }
    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        Instantiate(candyPrefab, enemyPosition, Quaternion.identity, transform);
    }
}
