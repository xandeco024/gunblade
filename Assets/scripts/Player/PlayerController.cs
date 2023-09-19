using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{

    //componentes
    private Rigidbody2D rigid;
    private Animator anim;

    [SerializeField] GameObject[] Cards ;

    public int maxHealth = 100;
    public int currentHealth;

    public GameManagerScript gameManager;

    public LayerMask groundLayer;
    public bool isOnGround;
    public Transform feet;
    public float checkRadius;
    public static int NumberOfCoins;
    public TextMeshProUGUI coinsText;

    private bool canMove = true;
    private bool isDead = false;

    private void Awake()
    {
        NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManagerScript>();
        currentHealth = maxHealth;
        transform.position = new Vector2(gameManager.RespawnPoint.x,gameManager.RespawnPoint.y);
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
        }
    }
    void FixedUpdate()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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

    public bool isPlayerDead()
    {
        return isDead;
    }

    public bool canPlayerMove()
    {
        return canMove;
    }

    public void setPlayerCanMove(bool move)
    {
        canMove = move;
    }
}