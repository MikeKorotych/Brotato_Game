using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header(" Elements ")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootingPoint;

    [Header(" Pooling ")]
    private ObjectPool<Bullet> bulletPool;
    void Update()
    {
        AutoAim();
    }

    void Start()
    {
        bulletPool = new ObjectPool<Bullet>(CreateAction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private Bullet CreateAction()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Cofigure(this);

        return bulletInstance;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    protected override void ManageAttack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        int damage = GetDamage(out bool isCriticalHit);

        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, transform.up, isCriticalHit);

    }
}
