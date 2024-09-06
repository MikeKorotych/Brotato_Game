using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private float hitDetectionRadius;

    [Header(" Settings ")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;


    [Header(" Animations ")]
    [SerializeField] private float aimLerp;

    void Update()
    {
        AutoAim();

        Attack();
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position, hitDetectionRadius, enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i].gameObject);
        }
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector2.up;

        if(closestEnemy != null)
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private Enemy GetClosestEnemy()
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}
