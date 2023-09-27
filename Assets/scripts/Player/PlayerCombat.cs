using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Components")]
    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private CapsuleCollider2D playerCol;
    private SpriteRenderer playerSR;

    [Header("Hero Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;
    private float currentHealth;
    private bool isDead = false;
    private int facingDirection;
    

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float recoilForce;
    [SerializeField] float shotCD;
    [SerializeField] float dashDuration;
    [SerializeField] private float reloadTime;
    private bool fireTrigger = false;
    private bool isShooting = false;
    private float shotCDCounter;
    private bool canShoot;
    private Vector2 firePoint;
    private Vector2 fireDirection;
    private int rotation = 0;

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

        firePoint = new Vector2(1, 0);
    }

    void Update()
    {
        inputCheck();

        if(Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1, true);
        }

        if (currentHealth <= 0)
        {
            // = true;
        }

        if(transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection= -1;

        attack(attackTrigger);

        if(Input.GetKeyDown(KeyCode.W))
        {
            rotation = 90;
            fireDirection = new Vector2(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rotation = 180;
            fireDirection = new Vector2(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            rotation = 270;
            fireDirection = new Vector2(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rotation = 0;
            fireDirection = new Vector2(1, 0);
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            rotation = 45;
            fireDirection = new Vector2(0.707f, 0.707f);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            rotation = 135;
            fireDirection = new Vector2(-0.707f, 0.707f);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            rotation = 225;
            fireDirection = new Vector2(-0.707f, -0.707f);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            rotation = 315;
            fireDirection = new Vector2(0.707f, -0.707f);
        }

        firePoint = new Vector2(transform.position.x, transform.position.y) + fireDirection * 2;
        DrawCircle(firePoint, attackRange, Color.red);
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
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(translatedAttackPoint, attackRange, enemyLayer);
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage, true);
                }
                Debug.Log("Atacou " + hitEnemies.Length + " inimigos");
                attackAnimIndex++;
                //Debug.Log("animação de ataque " + attackAnimIndex);
            }
            attackTrigger = false;
        }
    }

    void shoot(bool trigger)
    {

        DrawCircle(transform.position, 2, Color.cyan);

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

                //Debug.Log("atirou");
                StartCoroutine(DashCorroutine(dashDuration));

            }

            fireTrigger = false;
        }
    }

    IEnumerator DashCorroutine(float dashDuration)
    {
        //Debug.Log("Marco");

        isShooting = true;
        playerController.setPlayerCanMove(false);

        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint, Quaternion.Euler(0, 0, rotation));
        //bulletInstance.GetComponent<Rigidbody2D>().velocity = fireDirection * 20;

        playerRB.velocity = playerRB.velocity + -fireDirection * recoilForce;

        yield return new WaitForSeconds(dashDuration);

        //playerRB.velocity.Normalize();

        playerController.setPlayerCanMove(true);
        isShooting = false;

        //Debug.Log("Polo");
    }

    public void TakeDamage(float damage, bool knockBack)
    {
        currentHealth -= damage;
        StartCoroutine(Flash());

        if (knockBack) StartCoroutine(KnockBack());
    }

    IEnumerator Flash()
    {
        Color originalColor = playerSR.color;
        Color flashColor = new Color(255, 0, 0);

        playerSR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = originalColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = originalColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        playerSR.color = originalColor;
    }

    IEnumerator KnockBack()
    {
        playerController.setPlayerCanMove(false);
        playerRB.AddForce(new Vector2(-facingDirection, 1) * knockBackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackDuration);
        playerController.setPlayerCanMove(true);
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

    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
    }

    public void Heal(float health)
    {
        currentHealth += health;
    }
}