using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header(" Elements ")]
    private Player player;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        if (player != null)
            FollowPlayer();
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    private void FollowPlayer()
    {
        if (player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
    }
}
