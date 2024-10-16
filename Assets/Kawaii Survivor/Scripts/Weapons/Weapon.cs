using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }


    [Header(" Settings ")]
    [SerializeField] public float range;
    [SerializeField] protected LayerMask enemyMask;

    [Header(" Attack ")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;

    protected float attackTimer;


    [Header(" Critical ")]
    protected int criticalChance;
    protected float criticalPercent;

    [Header(" Animations ")]
    [SerializeField] protected float aimLerp;
    [SerializeField] protected Animator animator;

    [Header(" Level ")]
    public int Level { get; set; }

    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Vector2 targetUpVector = Vector2.up;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask);

        if (enemies.Length <= 0)
            return null;

        float minDistance = range;

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    protected virtual void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector2.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            SmoothAim(targetUpVector);
            ManageAttack();

            return;
        }
        SmoothAim(targetUpVector);
    }

    protected virtual void ManageAttack()
    {

    }

    private void SmoothAim(Vector2 targetUpVector)
    {
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if (Random.Range(0, 101) <= criticalChance)
        {
            isCriticalHit = true;
            return Mathf.RoundToInt(damage * criticalPercent);
        }

        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);

    protected void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);

        damage          = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay     = 1 / calculatedStats[Stat.AttackSpeed]; // if WeaponData.AttackSpeed = 2 => 1/2 = 0.5s per attack (and *1.33 per lvl)
        criticalChance  = Mathf.RoundToInt(calculatedStats[Stat.CriticalChance]); // at lvl1 => 1.33 - 0.20 => +13% crit chance per lvl
        criticalPercent = calculatedStats[Stat.CriticalPercent];
        range           = calculatedStats[Stat.Range];
    }

    internal void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;

        ConfigureStats();
    }
}
