using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack
    }

    private State state;

    [Header(" Elements ")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private BoxCollider2D hitCollider;
    [SerializeField] private float hitDetectionRadius;

    [Header(" Settings ")]
    [SerializeField] public float range;
    [SerializeField] private LayerMask enemyMask;

    [Header(" Attack ")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    protected float attackTimer;
    private List<Enemy> damagedEnemies = new List<Enemy>();

    [Header(" Animations ")]
    [SerializeField] protected float aimLerp;
    [SerializeField] private Animator animator;

    private void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                AutoAim();
                break;
            case State.Attack:
                Attacking();
                break;
        }
    }

    [NaughtyAttributes.Button]
    public void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;

        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;

        // clleat the attacked enemies list
        damagedEnemies.Clear();
    }

    private void Attack()
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(hitDetectionTransform.position,
            hitCollider.bounds.size,
            hitDetectionTransform.transform.localEulerAngles.z,
            enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            //1. is the enemy inside of the list ?
            if (!damagedEnemies.Contains(enemy))
            {
                // 2. if no let's attack him, and add to the list
                enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
                // 3. if yes, let's continue, check the next enemy
            }
        }
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector2.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack();
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);

        IncrementAttackTimer();
    }

    private void ManageAttack()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (hitCollider == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}
