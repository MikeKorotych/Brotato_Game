using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    [Header(" Components ")]
    protected EnemyMovement movement => GetComponent<EnemyMovement>();

    [Header(" Health ")]
    [SerializeField] protected int maxHealth;
    protected int health;

    [Header(" Elements ")]
    protected Player player => FindFirstObjectByType<Player>();

    [Header(" Spawn Sequence Related ")]
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected Collider2D collider;
    protected bool hasSpawned;

    [Header(" Attack ")]
    [SerializeField] protected float playerDetectionRadius;

    [Header(" Effects ")]
    [SerializeField] protected ParticleSystem passAwayParticles;

    [Header(" Actions ")]
    public static Action<int, Vector2, bool> onDamageTaken;

    [Header(" Debug ")]
    [SerializeField] protected bool drawGizmos = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        health = maxHealth;

        StartSpawnSequence();
    }

    // Update is called once per frame
    protected bool CanAttack()
    {
        return renderer.enabled;
    }

    private void StartSpawnSequence()
    {
        SetRenederersVisibility(false);

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

    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health);
        health -= realDamage;

        onDamageTaken.Invoke(damage, transform.position, isCriticalHit);

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
