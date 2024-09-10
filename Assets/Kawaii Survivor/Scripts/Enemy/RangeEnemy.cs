using System;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{

    [Header(" Components ")]
    private EnemyMovement movement => GetComponent<EnemyMovement>();
    private RangeEnemyAttack attack => GetComponent<RangeEnemyAttack>();

    [Header(" Elements ")]
    Player player => FindFirstObjectByType<Player>();

    [Header(" Spawn Sequence Related ")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;

    [Header(" Attack ")]
    [SerializeField] private float playerDetectionRadius;

    [Header(" Health ")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header(" Actions ")]
    public static Action<int, Vector2> onDamageTaken;

    [Header(" Debug ")]
    [SerializeField] private bool drawGizmos = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;

        SetRenederersVisibility(false);
        SpawnAnimation();

        attack.StorePlayer(player);
    }

    void Update()
    {
        if (!renderer.enabled)
            return;

        ManageAttack();
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }

    private void TryAttack()
    {
        attack.AutoAim();
    }
    private void SpawnAnimation()
    {
        // scale up and down
        Vector3 originalScale = spawnIndicator.transform.localScale;

        Vector3 targetScale = originalScale * .8f;

        spawnIndicator.transform.localScale = Vector3.zero;

        LeanTween.scale(spawnIndicator.gameObject, originalScale, .3f).setEase(LeanTweenType.easeOutBounce).setOnComplete(() =>
        {
            spawnIndicator.transform.localScale = originalScale;
            LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
                .setLoopPingPong(3)
                .setOnComplete(SpawnSequenceCompleted);
        });
    }

    private void SpawnSequenceCompleted()
    {
        SetRenederersVisibility(true);
        hasSpawned = true;

        collider.enabled = true;

        movement.StorePlayer(player);
    }

    private void SetRenederersVisibility(bool visible)
    {
        renderer.enabled = visible;
        spawnIndicator.enabled = !visible;
    }

    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken.Invoke(damage, transform.position);

        if (health <= 0)
            PassAway();
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
