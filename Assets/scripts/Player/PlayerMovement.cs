using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer playerSR;

    [Header("Movement Status")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallCheckDistance = 1.25f;
    [SerializeField] private float groundCheckDistance = 1.25f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    private int facingDirection = 1;
    private float wallJumpCD;

    private bool isWallSliding;
    private bool isWallJumping;
    private bool isJumping;
    private bool isRunning;

    private float horizontalInput;
    private bool jumpTrigger;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerController.canPlayerMove() && !playerController.isPlayerDead())
        {
            inputCheck();
            WallSlide();
        }
    }

    private void FixedUpdate()
    {
        Jump(jumpTrigger);
        Move(horizontalInput);
    }

    void inputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTrigger = true;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || !GroundCheck())
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        else horizontalInput = 0;

    }

    void Move(float horizontalInput)
    {
        if(!isWallSliding && !isWallJumping) 
        {
            Vector2 playerVelocity = playerRB.velocity;
            playerVelocity.x = (horizontalInput * moveSpeed);
            playerRB.velocity = playerVelocity;
            if (horizontalInput > 0 && facingDirection == -1) Flip();
            else if (horizontalInput < 0 && facingDirection == 1) Flip();

            isRunning = true;
        }

        else isRunning = false;
    }

    void Jump(bool trigger)
    {
        if(trigger)
        {
            if (GroundCheck() && !isWallSliding)
            {
                playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                isJumping = true;
            }

            else if (!GroundCheck() && isWallSliding)
            {
                Flip();
                playerRB.velocity = new Vector2(0, 0);
                playerRB.AddForce(new Vector2(10 * facingDirection, 10), ForceMode2D.Impulse);

                isWallJumping = true;
            }
        }

        if(isJumping && GroundCheck()) isJumping = false;

        if (isWallJumping)
        {
            wallJumpCD += Time.deltaTime;

            if (WallCheck() || wallJumpCD >= 1f || GroundCheck())
            {
                isWallJumping = false;
                wallJumpCD = 0;
            }
        }

        jumpTrigger = false;
    }

    private void WallSlide()
    {
        if (WallCheck() && !GroundCheck())
        {
            isWallSliding = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, -wallSlideSpeed);
        }
        else
        {
            isWallSliding = false;
        }

        CancelSlide();
    }

    private void CancelSlide()
    {
        if(isWallSliding && Input.GetKeyDown(KeyCode.A) && facingDirection == 1)
        {
            Flip();
        }

        else if (isWallSliding && Input.GetKeyDown(KeyCode.D) && facingDirection == -1)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingDirection = -facingDirection;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private bool WallCheck()
    {
        Debug.DrawRay(transform.position, Vector3.right * wallCheckDistance * facingDirection, Color.blue);
        return Physics2D.Raycast(transform.position, transform.right * facingDirection, wallCheckDistance, wallLayer);
    }

    private bool GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green);
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    public bool IsWallJumping()
    {
        return isWallJumping;
    }

    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsWallSliding()
    {
        return isWallSliding;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}