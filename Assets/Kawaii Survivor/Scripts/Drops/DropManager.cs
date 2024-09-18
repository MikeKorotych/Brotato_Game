using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class DropManager : MonoBehaviour
{
    [Header(" Pooling ")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;

    [Header(" Elements ")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Candy.onCollected += ReleaseCandy;
        Cash.onCollected += ReleaseCash;
    }
    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Candy.onCollected -= ReleaseCandy;
        Cash.onCollected -= ReleaseCash;

    }
    private void Start()
    {
        candyPool = new ObjectPool<Candy>(CandyCreateAction, CandyActionOnGet, CandyActionOnRelease, CandyActionOnDestroy);
        cashPool = new ObjectPool<Cash>(CashCreateAction, CashActionOnGet, CashActionOnRelease, CashActionOnDestroy);
    }

    #region Candy pooling
    private Candy CandyCreateAction() => Instantiate(candyPrefab, transform);
    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);
    #endregion

    #region Cash pooling
    private Cash CashCreateAction() => Instantiate(cashPrefab, transform);
    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);
    private void CashActionOnDestroy(Cash cash) => Destroy(cash.gameObject);
    #endregion

    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCash = Random.Range(0, 101) <= 30;

        DroppableCurrency droppable = shouldSpawnCash ? cashPool.Get() : candyPool.Get();
        droppable.transform.position = enemyPosition;
    }

    private void ReleaseCandy(Candy candy) => candyPool.Release(candy);
    private void ReleaseCash(Cash cash) => cashPool.Release(cash);
}
