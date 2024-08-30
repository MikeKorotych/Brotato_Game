using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header(" Elements ")]
    Player player => FindFirstObjectByType<Player>();

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float meleeAttackDistance = .3f;


    [Header(" Effects ")]
    [SerializeField] private ParticleSystem passAwayParticles;


    [Header(" Debug ")]
    [SerializeField] private bool drawGizmos = true;

    void Start()
    {

    }

    void Update()
    {
        FollowPlayer();

        TryAttack();
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= meleeAttackDistance)
            PassAway();
    }

    private void PassAway()
    {
        passAwayParticles.transform.SetParent(null);
        passAwayParticles.Play();

        Destroy(gameObject);
    }

    private void FollowPlayer()
    {
        if (player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, meleeAttackDistance);
    }
}
