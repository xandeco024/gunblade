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
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    private int facingDirection = 1;
    private bool isGrounded;

    public float wallCheckDistance = 2;
    private bool wallDetected;

    private bool canWallJump;
    private bool canWallSlide;
    private bool IsWallSliding;

    public float WallSlideSpeed;

    public float movHorizontal;
    public float movVertical;

    public bool isOnRight = true;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GroundCheck();
        Jump();

        if (isGrounded && wallDetected)
        {
            if (isOnRight && movHorizontal < 0)
            {
                Flip();
            }
            else if (!isOnRight && movHorizontal > 0)
            {
                Flip();
            }
        }

        if (wallDetected && canWallSlide)
        {
            IsWallSliding = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * 0.1f);
        }
        else
        {
            IsWallSliding = false;
        }

        WallCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float horizontalInput;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        else horizontalInput = 0;

        Vector2 playerVelocity = playerRB.velocity;

        playerVelocity.x = (horizontalInput * moveSpeed);

        playerRB.velocity = playerVelocity;

        if (horizontalInput > 0 && facingDirection == -1) Flip();
        else if (horizontalInput < 0 && facingDirection == 1) Flip();
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded && !IsWallSliding)
            {
                playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

            else if (IsWallSliding)
            {
                playerRB.AddForce(new Vector2(1 * -facingDirection, 100), ForceMode2D.Impulse);
            }
        }

    }

    private void WallCheck()
    {
        wallDetected = Physics2D.Raycast(transform.position, transform.right * facingDirection, 1.25f, wallLayer);
        Debug.DrawRay(transform.position, Vector3.right * 1.25f * facingDirection, Color.blue);

        if (!isGrounded) canWallSlide = true;
        else canWallSlide = false;
    }

    void Flip()
    {
        facingDirection = -facingDirection;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green);
        if (hit.collider != null) isGrounded = true;
        else isGrounded = false;
    }

    private void OnDrawGizmos()
    {

    }
}
