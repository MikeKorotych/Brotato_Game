using UnityEngine;
using UnityEngine.Pool;
public class DamageTextManager : MonoBehaviour
{


    [Header(" Elements ")]
    [SerializeField] private DamageText damageTextPrefab;


    [Header(" Pooling ")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
    }

    // Start вызывается перед первым кадром
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateAction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateAction()
    {
        return Instantiate(damageTextPrefab, transform);
    }
    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    private void EnemyHitCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        // set position
        Vector3 spawnPosition = enemyPos + new Vector2(Random.Range(-.3f, .2f), Random.Range(1f, 1.5f));
        damageTextInstance.transform.position = spawnPosition;

        // randomize scale
        var randomScale = Random.Range(.7f, 1f);
        damageTextInstance.transform.localScale = new Vector2(randomScale, randomScale);

        //play animation
        damageTextInstance.PlayAnimation(damage, isCriticalHit);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}