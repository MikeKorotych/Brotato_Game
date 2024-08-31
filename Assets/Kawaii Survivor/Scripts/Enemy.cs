using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{

    [Header(" Components ")]
    private EnemyMovement movement => GetComponent<EnemyMovement>();

    [Header(" Elements ")]
    Player player => FindFirstObjectByType<Player>();

    [Header(" Spawn Sequence Related ")]
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;

    [Header(" Attack ")]
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float meleeAttackDistance = .3f;
    private float attackDelay;
    private float attackTimer;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem passAwayParticles;

    [Header(" Debug ")]
    [SerializeField] private bool drawGizmos = true;

    void Start()
    {
        SetRenederersVisibility(false);
        SpawnAnimation();

        attackDelay = 1f / attackFrequency;
    }

    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();
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

        movement.StorePlayer(player);
    }

    private void SetRenederersVisibility(bool visible)
    {
        renderer.enabled = visible;
        spawnIndicator.enabled = !visible;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= meleeAttackDistance)
            Attack();
        //PassAway();
    }

    private void Attack()
    {
        Debug.Log("--- Dealing " + damage + " damage to the player ---");
        attackTimer = 0;
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();

        Destroy(gameObject);
    }

    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeAttackDistance);
    }
}
