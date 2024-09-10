using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D) ,typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{

    [Header(" Elements ")]
    private Rigidbody2D rig;
    private Collider2D collider;
    private RangeEnemyAttack rangeEnemyAttack;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    private int damage;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
        StartCoroutine(ReleaseCoroutine());
    }

    IEnumerator ReleaseCoroutine()
    {
        yield return new WaitForSeconds(5);
        rangeEnemyAttack.ReleaseBullet(this);
    }

    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage;

        transform.right = direction;
        rig.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            //LeanTween.cancel(gameObject);
            StopCoroutine(ReleaseCoroutine());

            player.TakeDamage(damage);
            this.collider.enabled = false;

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }

    public void Cofigure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Reload()
    {
        rig.linearVelocity = Vector2.zero;
        collider.enabled = true;
    }
}
