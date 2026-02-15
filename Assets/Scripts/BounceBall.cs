using UnityEngine;

public class BounceBall : MonoBehaviour
{
    public int maxBounces = 3;
    public float playerBounceForce = 14f;

    private int bounceCount = 0;
    private Rigidbody2D rb;
	public Vector2 currentDirection;

    public float speed = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Player jumps on ball
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Launch player upward
                playerRb.linearVelocity *= -1f;
            }
           //Vector2.Reflect(currentDirection, collision.GetContact(0).normal)
           // Destroy(gameObject);
            return;
        }
       // else
        {
       // currentDirection = Vector2.Reflect(currentDirection, collision.GetContact(0).normal);
        //currentDirection = Vector2.Reflect(currentDirection, collision.GetContact(0).normal);
		//now the current direction is bouncing off a vector that includes itself, 
		// the FIRST contact point, and from the first contact point you are reflecting 
		// back on a vector 
        }
		//currentDirection.Normalize(); 
       // rb.linearVelocity = currentDirection * speed;
        

        // Count ground bounces
        if (!collision.gameObject.CompareTag("Player"))
        {
            bounceCount++;

            if (bounceCount >= maxBounces)
            {
               // Destroy(gameObject);
            }
        }
    }

    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    
    // This replaces gravity. It tells the ball: "Your direction is DOWN"
    currentDirection = Vector2.down; 
    
    // This gives it the initial "fall" speed
    rb.linearVelocity = currentDirection * playerBounceForce; 
}

}