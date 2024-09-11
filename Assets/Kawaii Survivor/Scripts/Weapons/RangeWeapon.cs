using UnityEngine;

public class RangeWeapon : Weapon
{


    [Header(" Elements ")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();

        Vector2 targetUpVector = Vector2.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageShooting();

            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    private void ManageShooting()
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
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Shoot(damage, transform.up);
        bulletInstance.transform.position = shootingPoint.position;
        bulletInstance.gameObject.SetActive(true);
    }
}
