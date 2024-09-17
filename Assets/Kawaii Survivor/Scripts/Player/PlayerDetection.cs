using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] CircleCollider2D playerCollider;

    //private void FixedUpdate()
    //{
    //    Collider2D[] candyColliders = Physics2D.OverlapCircleAll((Vector2)transform.position + playerCollider.offset, playerCollider.radius);

    //    foreach (Collider2D collider in candyColliders)
    //    {
    //        if (collider.TryGetComponent(out Candy candy))
    //        {
    //            Destroy(candy.gameObject);
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Candy candy))
        {
            if (!collider.IsTouching(playerCollider))
                return;

            candy.Collect(transform);
        }
    }
}
