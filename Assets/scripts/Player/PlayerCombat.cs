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

    [Header("Combat Stats")]
    [SerializeField] private float maxHealth;
    private float currentHealth;

    [SerializeField] private Vector3 attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;

    private int attackAnimIndex;
    private bool isAttacking;
    private bool isDead = false;
    private int facingDirection;

    private bool attackTrigger;
    [SerializeField] float attackDuration;
    private float attackDurationCounter;
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

    void inputCheck()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            attackTrigger = true;
        }
    }

    void attack(bool trigger)
    {
        Vector3 translatedAttackPoint = transform.position + attackPoint * facingDirection;
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