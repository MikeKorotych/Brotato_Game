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
        PlayerHealth.onAttackDodged += AttackDodgedCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback;
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
        damageTextInstance.PlayAnimation(damage.ToString(), isCriticalHit);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }

    private void AttackDodgedCallback(Vector2 playerPosition)
    {
        DamageText damageTextInstance = damageTextPool.Get();

        // set position
        Vector3 spawnPosition = playerPosition + new Vector2(Random.Range(-.3f, .2f), Random.Range(1f, 1.5f));
        damageTextInstance.transform.position = spawnPosition;

        //play animation
        damageTextInstance.PlayAnimation("Dodged", false);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}