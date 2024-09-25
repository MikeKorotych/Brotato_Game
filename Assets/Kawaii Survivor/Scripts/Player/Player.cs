using UnityEngine;

[RequireComponent(typeof(PlayerHealth), typeof (PlayerLevel))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header(" Elements ")]
    [SerializeField] private CircleCollider2D collider;
    private PlayerHealth playerHealth => GetComponent<PlayerHealth>();
    private PlayerLevel playerLevel => GetComponent<PlayerLevel>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    public Vector2 GetCenter()
    {
        return collider.bounds.center;
    }

    public bool HasLeveledUp()
    {
        return playerLevel.HasLeveledUp();
    }
}
