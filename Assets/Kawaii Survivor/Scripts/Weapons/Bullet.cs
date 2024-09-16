using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header(" Elements ")]
    private Rigidbody2D rig;
    private Collider2D collider;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private RangeWeapon rangeWeapon;
    private Enemy target;

    private bool isCriticalHit;
    private int damage;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cofigure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Shoot(int damage, Vector2 direction, bool isCriticalHit)
    {
        Invoke("Release", 1);

        this.damage = damage;
        this.isCriticalHit = isCriticalHit;

        transform.right = direction;
        rig.linearVelocity = direction * moveSpeed;
    }

    public void Reload()
    {
        target = null;

        rig.linearVelocity = Vector2.zero;
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (target != null)
            return;

        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();

            CancelInvoke();

            Attack(target);
            Release();
        }
    }

    private void Release()
    {
        if (!gameObject.activeSelf)
            return;

        rangeWeapon.ReleaseBullet(this);
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }
}
