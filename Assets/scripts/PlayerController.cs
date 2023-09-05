using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;
    //componentes
    private Rigidbody2D rigid;
    private Animator anim;

    [SerializeField] GameObject[] Cards ;

    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;

    public GameManagerScript gameManager;

    private bool canMove = true;
    public float velocity = 10f;
    public float jumpForce = 10f;
    public float WallSlideSpeed;

    public float movHorizontal;
    public float movVertical;
    public bool isOnRight = true;
    private int facingDiraction = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    public LayerMask groundLayer;
    public bool isOnGround;
    public Transform feet;
    public float checkRadius;

    public Transform wallCheck;
    public float wallCheckDistance;
    private bool IsWallDetected;

    private bool canWallJump;
    private bool canWallSlide;
    private bool IsWallSliding;

 
    

    public static int NumberOfCoins;
    public TextMeshProUGUI coinsText;
        
 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManagerScript>();
        currentHealth = maxHealth;
        transform.position = new Vector2(gameManager.RespawnPoint.x,gameManager.RespawnPoint.y);
    }
    private void Awake()
    {
        NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }
    private void CollisionCheck()
    {
        IsWallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, groundLayer);

        if (!isOnGround && rigid.velocity.y < 0)
            canWallSlide = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (isOnRight)

            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z ));

        else

            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y, wallCheck.position.z) );
    }

    void Update()
    {

        isOnGround = Physics2D.OverlapCircle(feet.position, checkRadius, groundLayer);

        if (!isDead) 
        {
            if (currentHealth == 60) 
            {
                Destroy(Cards[4].gameObject);
            }
            if (currentHealth == 40)
            {
                Destroy(Cards[3].gameObject);
            }
            if (currentHealth == 20)
            {
                Destroy(Cards[2].gameObject);
            }
            if (currentHealth == 0)
            {
                Destroy(Cards[1].gameObject); 
                Destroy(Cards[0].gameObject);
            }
   
            if (currentHealth <= 0)
            {
                isDead = true;
            }
            if (isDead)
            {
                gameManager.gameOver();
                //Destroy(this.gameObject);
            }
            
                coinsText.text = NumberOfCoins.ToString();

                movHorizontal = Input.GetAxis("Horizontal");

                if (Input.GetButtonDown("Jump") && isOnGround)
                {
                
                if (IsWallSliding && canWallJump)
                
                {
                    WallJump();
                }

                else if (isOnGround)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
                    }
                }

                if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0f)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.5f);
                }

                if (movHorizontal > 0 && !isOnRight)
                {
                    Flip();
                }
                if (movHorizontal < 0 && isOnRight)
                {
                    Flip();
                }

            if (isOnGround && IsWallDetected) 
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

                Animations();

                void Flip()
                {
                    facingDiraction = facingDiraction * -1;
                    isOnRight = !isOnRight;
                    transform.Rotate(0, 180, 0);
                }
        }
    }
    void FixedUpdate()
    {
        if (IsWallDetected && canWallSlide)
        {
            IsWallSliding = true;
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.1f);
        }
        else
        {
            IsWallSliding = false;
            Move();
        }
        CollisionCheck();

    }
    private void WallJump() 
    {
        Vector2 direction = new Vector2(wallJumpDirection.x * -facingDiraction, wallJumpDirection.y);

        rigid.AddForce(direction, ForceMode2D.Impulse);
    }

    private void Move()
    {
        if(canMove)
        rigid.velocity = new Vector2(velocity * movHorizontal, rigid.velocity.y);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Animations()
    {
        // uma serie de else ifs para definir as animações do nosso jogo 
        if(movHorizontal != 0)
        {
  
            anim.SetInteger("Transition", 1);
        }
        else if (movHorizontal == 0)
        {
            anim.SetInteger("Transition", 0);
        }
        
        if (!isOnGround)
        {
            anim.SetInteger("Transition", 2);
        }
        anim.SetBool("IsWallsliding", IsWallSliding);
    }
 


    private void OnTriggerEnter2D(Collider2D other)
    {
        //rigid.AddForce(transform.forward * 8, ForceMode2D.Impulse);

        if (other.CompareTag("Bullet"))
        {
            rigid.AddForce(transform.up * 8, ForceMode2D.Impulse);
            //KnockBack(other);
        }
    }

    void KnockBack(Collider2D coll)
    {
        GameObject bullet = coll.gameObject;

        if (bullet.transform.position.x > transform.position.x)
        {
            print("Mode1");
            //rigid.velocity = new Vector2(-25, 1);
            rigid.AddForce(transform.up * 10, ForceMode2D.Impulse);
        }
        else if (bullet.transform.position.x <= transform.position.x)
        {
            print("Mode2");
            //rigid.velocity = new Vector2(25, 1);
            rigid.AddForce(transform.up * 10, ForceMode2D.Impulse);
        }
    }
}