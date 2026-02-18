using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Ball : MonoBehaviour
{
    public int maxBounces = 3;
    public float playerBounceForce = 14f;

    private int bounceCount = 0;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player lands on the ball
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // launch player upward
                playerRb.linearVelocity = new Vector2(
                    playerRb.linearVelocity.x,
                    playerBounceForce
                );
            }

            Destroy(gameObject);
            return;
        }

        // Otherwise it's a wall/ground bounce
        bounceCount++;

        if (bounceCount >= maxBounces)
        {
            Destroy(gameObject);
        }
    }
}
