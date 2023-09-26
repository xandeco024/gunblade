using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer playerSR;

    [Header("Hero Stats")]
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private LayerMask enemyLayer;
    private bool isDead = false;
    private int facingDirection;

    [Header("Gun")]
    [SerializeField] private Vector2 firePoint;
    [SerializeField] private float recoilForce;
    private bool fireTrigger = false;
    private bool isShooting = false;
    [SerializeField] float shotCD;
    [SerializeField] float shotDuration;
    private float shotCDCounter;
    private float shotDurationCounter;
    private bool canShoot;

    [Header("Blade")]
    [SerializeField] private Vector2 attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] float attackDuration;
    private float attackDurationCounter;
    private bool attackTrigger = false;
    private bool isAttacking = false;
    private int attackAnimIndex;
    private bool canAttack;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        inputCheck();


        if (currentHealth <= 0)
        {
            isDead = true;
        }

        if(transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection= -1;

        attack(attackTrigger);
    }

    private void FixedUpdate()
    {
        shoot(fireTrigger);
    }

    void inputCheck()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attackTrigger = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            fireTrigger = true;
        }
    }

    void attack(bool trigger)
    {
        Vector2 translatedAttackPoint = new Vector2(transform.position.x, transform.position.y) + attackPoint * facingDirection;
        DrawCircle(translatedAttackPoint, attackRange, Color.red);

        if(isAttacking)
        {
            attackDurationCounter += Time.deltaTime;

            if(attackDurationCounter >= attackDuration)
            {
                isAttacking = false;
                attackDurationCounter = 0;
            }
        }

        if (attackAnimIndex >= 3) attackAnimIndex = 0;

        if(trigger)
        {
            if(!isAttacking)
            {
                isAttacking = true;
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayer);
                foreach (Collider2D enemy in hitEnemies)
                {
                    //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                }
                Debug.Log("Atacou " + hitEnemies.Length + " inimigos");
                attackAnimIndex++;
                Debug.Log("animação de ataque " + attackAnimIndex);
            }
            attackTrigger = false;
        }
    }

    void shoot(bool trigger)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Pega a posição do mouse na tela
        mousePosition.z = 0; // Define a coordenada z da posição do mouse para 0, já que estamos trabalhando em 2D

        Vector3 direction = (mousePosition - transform.position).normalized; // Calcula a direção do cursor do mouse em relação à posição do personagem

        Vector3 circlePosition = transform.position + direction * 3;

        Vector2 translatedFirePoint = new Vector2(transform.position.x, transform.position.y) + new Vector2(firePoint.x * facingDirection, firePoint.y);
        DrawCircle(circlePosition, attackRange, Color.red);

        DrawCircle(transform.position, 3, Color.cyan);

        if (isShooting)
        {
            shotDurationCounter += Time.deltaTime;

            if (shotDurationCounter >= shotDuration)
            {
                isShooting = false;
                shotDurationCounter = 0;
            }
        }

        if (!canShoot)
        {
            shotCDCounter += Time.deltaTime;

            if (shotCDCounter >= shotCD)
            {
                canShoot = true;
                shotCDCounter = 0;
            }
        }


        if (trigger)
        {
            if(!isShooting && canShoot)
            {
                isShooting = true;
                Debug.Log("atirou");
                //playerController.setPlayerCanMove(false);
                playerRB.AddForce(-direction * recoilForce, ForceMode2D.Impulse);
            }

            fireTrigger = false;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    void KnockBack(Collider2D coll)
    {
        GameObject bullet = coll.gameObject;

        if (bullet.transform.position.x > transform.position.x)
        {
            print("Mode1");
            //rigid.velocity = new Vector2(-25, 1);
            playerRB.AddForce(transform.up * 10, ForceMode2D.Impulse);
        }
        else if (bullet.transform.position.x <= transform.position.x)
        {
            print("Mode2");
            //rigid.velocity = new Vector2(25, 1);
            playerRB.AddForce(transform.up * 10, ForceMode2D.Impulse);
        }
    }

    void DrawCircle(Vector3 position, float radius, Color color)
    {
        int segments = 360;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 point1 = new Vector3(x, y, 0) + position;
            angle += 360f / segments;
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            Vector3 point2 = new Vector3(x, y, 0) + position;
            Debug.DrawLine(point1, point2, color);
        }
    }
}