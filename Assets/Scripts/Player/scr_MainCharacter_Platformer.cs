using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class scr_Player_Main : MonoBehaviour 
{
    public Rigidbody2D rb;

    public float GroundCheckRadius = 0.2f;
    public bool onGround = false;

    public Transform GroundCheck;
    public Animator animator;

    public bool faceRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
        WallCheckRadius = WallCheck.GetComponent<CircleCollider2D>().radius; 
        gravityDef = rb.gravityScale;
    }

    void Update()
    {
        CheckingGround();
        HandleWallSlide();
        Walk();
        Reflect();
        Jump();
        WallJump();
        Dash();
        CheckingWall();
    }

    public float realSpeed = 6f;
    public float runSpeedMultiplier = 2;
    public float smoothTime = 0.1f;

    private Vector2 moveVector;
    private Vector2 currentVelocity;

    void Walk()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");

        // Определяем скорость в зависимости от нажатия Shift
        float speed = Input.GetKey(KeyCode.LeftShift) ? realSpeed * runSpeedMultiplier : realSpeed;

        // Сглаживание движения с помощью Lerp
        float targetVelocityX = moveVector.x * speed;
        rb.linearVelocity = new Vector2(
            Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref currentVelocity.x, smoothTime),
            rb.linearVelocity.y
        );
        animator.SetFloat("moveX", Mathf.Abs(moveVector.x));
    }

    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    public static float jumpForce = 210f;
    private bool jumpControl;
    private float jumpIteration = 0;
    public float jumpValueIteration = 60;

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (onGround) 
            { 
                jumpControl = true; 
            }
            
        }
        else { jumpControl = false; }
        if (jumpControl)
        {
            if (jumpIteration++ < jumpValueIteration)
            {
                rb.AddForce(Vector2.up * jumpForce / jumpIteration);
            }
        }
        else { jumpIteration = 0; }
    }
    public int dashImpulse = 500;
    private bool dashLock = false;

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !dashLock && stamina * 2 >= maxStamina)
        {
            dashLock = true;
            rb.linearVelocity = new Vector2(0, 0);

            if (faceRight)
            {
                rb.AddForce(Vector2.right * dashImpulse, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.left * dashImpulse, ForceMode2D.Impulse);
            }
            Invoke("DashUnlock", 2f);
        }
    }
    public LayerMask Ground;
    void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        animator.SetBool("onGround", onGround);
        if (onGround && stamina < maxStamina)
        {
            stamina += 2*Time.deltaTime; ;
        }
    }
    private void DashUnlock() => dashLock = false; 

    public float climbForce = 10f;
    public float slideSpeed = 20f;
    private float gravityDef;


    public bool onWall;
    private float WallCheckRadius;
    public Transform WallCheck;


    private bool blockMoveXforJump;
    public float jumpWallTime = 0.5f;
    private float timerJumpWall;
    public Vector2 jumpAngle = new Vector2(3.5f, jumpForce/100 );
    void WallJump()
    {
        if (onWall && !onGround && Input.GetKeyDown(KeyCode.Space) && stamina * 4 >= maxStamina)
        {
            blockMoveXforJump = true;
            moveVector.x = 0;
            stamina -= 1;
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
            rb.gravityScale = gravityDef;
            rb.linearVelocity = new Vector2(0, 0);
            rb.linearVelocity = new Vector2(transform.localScale.x * jumpAngle.x, jumpAngle.y);
        }
        if (blockMoveXforJump && (timerJumpWall += Time.deltaTime) >= jumpWallTime)
        {
            if (onWall || onGround || Input.GetAxisRaw("Horizontal") != 0)
            {
                blockMoveXforJump = false;
                timerJumpWall = 0;
            }
        }
    }
    void CheckingWall()
    {
        onWall = Physics2D.OverlapCircle(WallCheck.position, WallCheckRadius, Ground);
    }

    public float maxStamina = 4f; // Максимальное значение силы
    public float stamina;        // Текущее значение силы

    void HandleWallSlide()
    {
        if (onWall && !onGround)
        {
            if (stamina > 0)
            {
                if (Input.GetKey(KeyCode.W) && stamina > maxStamina/2)
                {
                    rb.AddForce(Vector2.up * climbForce);
                }
                if(!onGround) stamina -= Time.deltaTime;
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -slideSpeed);
            }
        }
    }
}