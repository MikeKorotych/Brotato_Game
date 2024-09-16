using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
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

    [Header(" Settings ")]
    private List<Enemy> damagedEnemies = new List<Enemy>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
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

    protected override void AutoAim()
    {
        base.AutoAim();

        IncrementAttackTimer();
    }

    protected override void ManageAttack()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        Debug.Log("--- melee weapon ---");
        }


    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
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
                int damage = GetDamage(out bool isCriticalHit);

                // 2. if no let's attack him, and add to the list
                enemy.TakeDamage(damage, isCriticalHit);
                damagedEnemies.Add(enemy);
                // 3. if yes, let's continue, check the next enemy
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitCollider == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, range);
    }
}
