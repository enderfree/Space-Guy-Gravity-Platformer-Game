using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Run  
    public float speed = 1f;
    public float accel = 2f;
    public float deccel = 2f;
    //Jump
    public float jumpPower = 1f;

    float moveDirection = 0f;

    bool grounded = true;
    [SerializeField] bool moveInputed = false;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = 0f;
           
        //Holding Movement Input
        if (Input.GetKey("left") 
            || Input.GetKey(KeyCode.A))
        {
            moveDirection = -1f;
            moveInputed = true;
        }
        else if (Input.GetKey("right") 
            || Input.GetKey(KeyCode.D))
        {
            moveDirection = 1f;
            moveInputed = true;
        }

        if (Input.GetKeyUp("left") 
            || Input.GetKeyUp(KeyCode.A) 
            || Input.GetKeyUp("right") 
            || Input.GetKeyUp(KeyCode.D))
        {
            moveInputed = false;

        }

        //Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            grounded = false;
        }
        //Holding Jump Button
        if (Input.GetKey(KeyCode.Space) && grounded == false)
        {

        }

    }

    private void FixedUpdate()
    {
        Vector3 dir = new Vector3(moveDirection, 0, 0);

        //Movement Acceleration Boost
        if ((Input.GetKeyDown("left")
            || Input.GetKeyDown(KeyCode.A))
            && rb.linearVelocity.x >= 0f)
        {
            moveDirection = -1f;
            rb.AddForce(dir * accel * Time.deltaTime, ForceMode.Impulse);
            moveInputed = true;
        }
        else if ((Input.GetKeyDown("right")
            || Input.GetKeyDown(KeyCode.D))
            && rb.linearVelocity.x <= 0f)
        {
            moveDirection = 1f;
            rb.AddForce(dir * accel * Time.deltaTime, ForceMode.Impulse);
            moveInputed = true;
        }


        //Movement Force
        rb.AddForce(dir * speed * Time.deltaTime);

        //Decceleration
        if (moveInputed == false)
        {
            rb.AddForce((-rb.linearVelocity.x * deccel),0,0);
        }
    }
    
    
    private void OnCollisionEnter(Collision collision)
    {
        //Jump availibilty check
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }  
    }

    private void Jump()
    {
      //  rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
       // grounded = false;
    }

}


