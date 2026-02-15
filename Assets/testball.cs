using UnityEngine;

public class testball : MonoBehaviour
{
    public int maxBounces = 3;
    public float playerBounceForce = 14f;

    private int bounceCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        // Player jumps on ball
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Launch player upward
                playerRb.linearVelocity = new Vector3(
                    playerRb.linearVelocity.x,
                    playerBounceForce,
                    playerRb.linearVelocity.z
                );
            }

            Destroy(gameObject);
            return;
        }

        // Count ground bounces
        if (!collision.gameObject.CompareTag("Player"))
        {
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);
            }
        }
    }
}
