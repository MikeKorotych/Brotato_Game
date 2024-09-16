using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    [Header(" Settings ")]
    [SerializeField] public float range;
    [SerializeField] protected LayerMask enemyMask;

    [Header(" Attack ")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;

    protected float attackTimer;

    [Header(" Animations ")]
    [SerializeField] protected float aimLerp;
    [SerializeField] protected Animator animator;

    private void Start()
    {

    }

    void Update()
    {

    }

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

        if (Random.Range(0, 101) <= 50)
        {
            isCriticalHit = true;
            return damage * 2;
        }



        return damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
