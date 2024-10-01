using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerStatsDependency
{

    [Header(" Elements ")]
    [SerializeField] private MobileJoystick playerJoystick;
    private Rigidbody2D rig;

    [Header(" Settings ")]
    [SerializeField] private float baseMoveSpeed;
    private float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>(); 
        rig.linearVelocity = Vector2.right;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.linearVelocity = playerJoystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedPercent = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;

        Debug.Log("--- move speed percent: " + moveSpeedPercent + " ---");

        moveSpeed = baseMoveSpeed * (1 + moveSpeedPercent);
    }
}
