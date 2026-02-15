using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public InputSet inputSet;
    
    //Run  
    public float topSpeed = 1f;
    private float myXVelocity = 0f;
    public float boost = 5f;
    public float accel = 2f;
    public float deccel = 2f;
    float moveDirection = 0f;
    //Jump
    public float jumpPower = 1f;
    private float myYVelocity = 0f;
    public float jumpYVelocity = 10f; //jump initial speed
    private float jumpCancelYVelocity = 5f; //speed to which the jump is cancelable
    //Gravity
    public float gravity = -10f;
    public float terminalYVelocity = -53f;
    public float myDefaultVelocity = -1f;

    //State Machine
    private string currentState = "none";
    bool grounded = true;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        ApplyGravity();
        RunStates();
        MovePlayer();
        Debug.Log(currentState);
        Debug.Log(moveDirection);
    }
    
    
    private void OnCollisionEnter2D(Collision2D col) 
    {
        //Check if grounded
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            Debug.Log("Grounded");
        }  
    }
    private void RunStates()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");

        //Jump Input
        if (Input.GetButton("Jump") && currentState != "IsJumping")
        {
            currentState = "IsJumping";
            StartJump();
        }
        else if (moveDirection != 0f && currentState !="IsRunning" && currentState != "IsJumping")
        {
            StartRunning();
        }
        
        //Run Ongoing State
        if (currentState == "IsJumping") { Jump(); }
        else if (currentState == "IsRunning") { Running(); }

    }
    
    //Jump Functions
    private void StartJump()
    {
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        currentState = "IsJumping";
        grounded = false;
    }
    private void Jump()
    {
        //Play Jump animation here
        
        if (!Input.GetButton("Jump") && myYVelocity > jumpCancelYVelocity)
        {
            myYVelocity = jumpCancelYVelocity;
        }
        myXVelocity = rb.linearVelocityX;

        //just the run code
        if (moveDirection > 0)
        {
            if (myXVelocity < topSpeed)
            {
                rb.AddForceX((myXVelocity += accel), ForceMode2D.Force);
            }
            else
            {
                rb.AddForceX(myXVelocity, ForceMode2D.Force);
            }

        }
        else if (moveDirection < 0)
        {

            if (myXVelocity > -topSpeed)
            {
                rb.AddForceX((myXVelocity -= accel), ForceMode2D.Force);
            }
            else
            {
                rb.AddForceX(myXVelocity, ForceMode2D.Force);
            }
        }
            //exit condition
            if (grounded == true)
        {
            StopJump();
        }
    }
    private void StopJump()
    {
        currentState = "none";
    }
    
    //Running Functions
    private void StartRunning()
    {
       
        //Vector2 dir = new Vector2(moveDirection, 0);
        
        if (moveDirection == -1f)
        {
            rb.AddForceX( -boost, ForceMode2D.Impulse);
        }
        else if (moveDirection == 1f)
        {
            rb.AddForceX( boost, ForceMode2D.Impulse);
        }
        currentState = "IsRunning";
    }
    private void Running()
    {

        myXVelocity = rb.linearVelocityX;
        //Play Run animation here
        if (moveDirection > 0)
        {
            if (myXVelocity < topSpeed)
            { 
                rb.AddForceX((myXVelocity += accel), ForceMode2D.Force); 
            }
            else
            {
                rb.AddForceX(myXVelocity, ForceMode2D.Force);
            }

        }
        else if (moveDirection < 0)
        {

            if (myXVelocity > -topSpeed)
            {
                rb.AddForceX((myXVelocity -= accel), ForceMode2D.Force);
            }
            else
            {
                rb.AddForceX(myXVelocity, ForceMode2D.Force);
            }
        //{if (myXVelocity > -topSpeed){myXVelocity -= accel * Time.deltaTime;} else{ myXVelocity = -topSpeed;}
        //rb.AddForceX(myXVelocity - accel, ForceMode2D.Force);
            }
        else if (moveDirection == 0)
        {
            //   myXVelocity = 0;
               StopRunning();
        }

    }
    private void StopRunning()
    {
       
        rb.AddForceX((-myXVelocity * deccel));
        if (rb.linearVelocityX == 0f && currentState != "IsJumping")
        {
            currentState = "none";
        }

    }
    
    //Movement Funtions
    private void MovePlayer()
    {
        //Update controller position
        Vector2 dir = new Vector2(myXVelocity,myYVelocity);

        //Movement Force
        rb.AddForce(dir * Time.deltaTime, ForceMode2D.Force);
    }
    void ApplyGravity()
    {
        if (!grounded)
        {
            if(myYVelocity > terminalYVelocity)
            {
                myYVelocity += gravity * Time.deltaTime;
            }
            else
            {
                myYVelocity = terminalYVelocity;
            }
        }
        else
        {
            myYVelocity = myDefaultVelocity;
        }
    }

}


