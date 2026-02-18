using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputSet inputSet;
    
    //Run  
    public float topSpeed = 10f;
    public float boost = 7f;
    public float groundAccel = 30f;
    public float groundDecel = 30f;
    public float airAccel = 30f;
    public float airDecel = 30f;
    float moveDirection = 0f;
    //--Jump--
    // Ground Detection
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    //Coyote Time and Jump Buffer
    private float coyoteCounter;
    [SerializeField] private float coyoteTime = 0.2f;
    private float jumpBufferTimer;
    [SerializeField] private float jumpBuffer = 0.2f;
    //Jump Physics
    public float jumpPower = 10f;
    private float myYVelocity = 0f; //jump initial speed //speed to which the jump is cancelable
    public float jumpCancelMultiplier = 0.7f; 
    private bool isJumpPressed = false;
    private bool isJumpReleased = false;
    //Gravity Physics
    [SerializeField] private float gravity = -20f;
    private float terminalYVelocity = -53f;
    private float myDefaultVelocity = -1f;

    //State Machine
    private string currentState = "none";
    Rigidbody2D rb;

    //Animations
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float verticalVelocity;

    private void OnEnable()
    {
        inputSet.Player.Jump.Enable();
        inputSet.Player.Jump.performed += onJumpPerformed;
        inputSet.Player.Jump.canceled += onJumpCanceled;

    }

    private void OnDisable()
    {
        inputSet.Player.Jump.performed -= onJumpPerformed;
        inputSet.Player.Jump.canceled -= onJumpCanceled;
        inputSet.Player.Jump.Disable();
    }
    private void onJumpPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumpPressed = true;
        }
    }
    private void onJumpCanceled(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {

            isJumpReleased = true;
            
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        inputSet = new InputSet();
    }

    private void Update()
    {
        InputCheck();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        HorizontalMovement();
        RunStates();
        //MovePlayer();
        // Debug.Log(currentState);
        //Debug.Log(moveDirection);
        AnimationController();
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    private void InputCheck()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        //Coyote Check
        if (isGrounded())
        { coyoteCounter = coyoteTime; }
        else
        { coyoteCounter -= Time.deltaTime; }

        //Jump Input
        if (isJumpPressed)
        {
            jumpBufferTimer = jumpBuffer;
            isJumpPressed = false;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }
    private void RunStates()
    {


        if (jumpBufferTimer > 0f && coyoteCounter > 0f)
        {
            coyoteCounter = 0f;
            jumpBufferTimer = 0f;
            StartJump();

        }
        else if (moveDirection != 0f && currentState != "IsRunning" && currentState != "IsJumping")
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
        //Debug.LogWarning("Jump Script");
        currentState = "IsJumping";

        //rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);

    }
    private void Jump()
    {
        //Horizontal Movement
        //myXVelocity = rb.linearVelocityX;
        //float targetSpeed = moveDirection * topSpeed;
        //float speedDif = targetSpeed - myXVelocity;
        //rb.AddForceX((speedDif), ForceMode2D.Force);
        //rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocityY);

        //Jump Release Check
        if (isJumpReleased && rb.linearVelocityY > 0)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * jumpCancelMultiplier);
            isJumpReleased = false;
            
        }
       
        //exit condition
        if (isGrounded() && rb.linearVelocityY <=0f)
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
        float boostedVelocity = boost * moveDirection;

        if (Mathf.Abs(rb.linearVelocityX) < 0.1f)
        {
            rb.linearVelocity = new Vector2(boostedVelocity, rb.linearVelocityY);
        }

        currentState = "IsRunning";
    }
    private void Running()
    {
        float targetSpeed = moveDirection * topSpeed;

        float accelRate;

        if (moveDirection != 0)
            accelRate = isGrounded() ? groundAccel : airAccel;
        else
            accelRate = isGrounded() ? groundDecel : airDecel;

        if (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.linearVelocityX) && moveDirection != 0)
        {
            accelRate = groundDecel * 1.5f; 
        }

        float newX = Mathf.MoveTowards(
            rb.linearVelocityX,
            targetSpeed,
            accelRate * Time.fixedDeltaTime
        );

        //rb.linearVelocity = new Vector2(newX, rb.linearVelocityY);

        if (moveDirection == 0 && Mathf.Abs(newX) < 0.01f)
            currentState = "none";
    }

    //Movement Funtions
    private void HorizontalMovement()
    {
        float targetSpeed = moveDirection * topSpeed;

        float accelRate;

        if (moveDirection != 0)
            accelRate = isGrounded() ? groundAccel : airAccel;
        else
            accelRate = isGrounded() ? groundDecel : airDecel;

        if (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.linearVelocityX) && moveDirection != 0)
        {
            accelRate = groundDecel * 1.5f;
        }

        float newX = Mathf.MoveTowards(
            rb.linearVelocityX,
            targetSpeed,
            accelRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(newX, rb.linearVelocityY);
    }

    void ApplyGravity()
    {
        //myYVelocity = rb.linearVelocityY;
        if (!isGrounded())
        {
            if(myYVelocity > terminalYVelocity)
            {
                myYVelocity += gravity * Time.deltaTime;
            }
            else
            {
                myYVelocity = terminalYVelocity;
            }
            //For Animations
            if (currentState != "IsJumping")
            {
                currentState = "IsJumping";
            }
        }
        else
        {
            myYVelocity = myDefaultVelocity;
        }

        rb.AddForceY(myYVelocity, ForceMode2D.Force);
    }

    //Animation Functions
    void AnimationController()
    {
        verticalVelocity = rb.linearVelocityY;

        if (currentState == "none")
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingMid", false);
            animator.SetBool("isJumpingDown", false);
        }
        else if (currentState == "IsRunning")
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingMid", false);
            animator.SetBool("isJumpingDown", false);
        }
        else if (currentState == "IsJumping")
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);
            AnimationJumping();
        }
        AnimationTurning();
        //insert funny thing here
    }

    void AnimationTurning()
    {
        if (moveDirection == -1 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
            if (currentState == "IsRunning")
            {
                animator.SetTrigger("isTurning");
            }
        }
        if (moveDirection == 1 && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
            if (currentState == "IsRunning")
            {
                animator.SetTrigger("isTurning");
            }
        }
    }

    void AnimationJumping()
    {
        if (verticalVelocity >= 2)
        {
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isJumpingMid", false);
            animator.SetBool("isJumpingDown", false);
        }
        else if (verticalVelocity < 2 && verticalVelocity > -2)
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingMid", true);
            animator.SetBool("isJumpingDown", false);

        }
        else if (verticalVelocity <= -2)
        {
            animator.SetBool("isJumpingUp", false);
            animator.SetBool("isJumpingMid", false);
            animator.SetBool("isJumpingDown", true);

        }
    }
}


