using System;
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
    

    private bool firstKeyPressed = false;
    private float timeFirstKeyPressed;

    [Header("Hero Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isDead = false;
    private int facingDirection;

    
    

    [Header("Gun")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float recoilForce;
    [SerializeField] private int ammo = 2;
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


    /*isso aqui serve para ajustar o tanto munição que ele possui
      para poder atirar
     */

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

    private PlayerMovement playerMovment;
    //private PlayerController playerController;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        playerSR = GetComponent<SpriteRenderer>();

        playerMovment = GetComponent<PlayerMovement>();

        currentHealth = maxHealth;

        firePoint = new Vector2(1, 0);
    }

    void Update()
    {
        inputCheck();

        if (ammo > 2) ammo = 2;
        Debug.Log(ammo);

        if (Input.GetKeyDown(KeyCode.K)) TakeDamage(currentHealth, false);

        if (Input.GetKeyDown(KeyCode.L))
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

        else if (CheckKeys(KeyCode.A, KeyCode.W, .2f))
        {
            rotation = 135;
            fireDirection = new Vector2(-0.707f, 0.707f); // isso aq n funciona direito n�o, tem q pensar num jeito melhor
        }

        else if (CheckKeys(KeyCode.D, KeyCode.W, .2f))
        {
            rotation = 45;
            fireDirection = new Vector2(0.707f, 0.707f);
        }

        else if (CheckKeys(KeyCode.A, KeyCode.S, .2f))
        {
            rotation = 225;
            fireDirection = new Vector2(-0.707f, -0.707f);
        }

        else if (CheckKeys(KeyCode.D, KeyCode.S, .2f))
        {
            rotation = 315;
            fireDirection = new Vector2(0.707f, -0.707f);
        }

        firePoint = new Vector2(transform.position.x, transform.position.y) + fireDirection * 4;
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
                //Debug.Log("anima��o de ataque " + attackAnimIndex);
            }
            attackTrigger = false;
        }
    }

    void shoot(bool trigger)
    {

        DrawCircle(transform.position, 4, Color.cyan);

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
            if (ammo > 0)
            {
                if (!isShooting && canShoot && !playerMovment.GroundCheck())
                {
                    ammo--;
                    //Debug.Log("atirou");
                    StartCoroutine(AmmoCD(reloadTime));
                    StartCoroutine(DashCorroutine(5 * dashDuration));

                }

                if (!isShooting && canShoot && playerMovment.GroundCheck())
                {
                    ammo--;
                    StartCoroutine(AmmoCD(reloadTime));
                    StartCoroutine(DashCorroutine(dashDuration));
                }
            }

            fireTrigger = false;
        }
    }

    IEnumerator AmmoCD (float time)
    {
        yield return new WaitForSeconds(time);
        ammo++;
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

    public void TakeDamage(int damage, bool knockBack)
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

    public int GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
    }

    public void Heal(int health)
    {
        currentHealth += health;
    }

    public bool CheckKeys(KeyCode firstKey, KeyCode secondKey, float timeLimit)
    {
        if (Input.GetKeyDown(firstKey))
        {
            firstKeyPressed = true;
            timeFirstKeyPressed = Time.time;
        }

        if (firstKeyPressed && Input.GetKeyDown(secondKey))
        {
            if (Time.time - timeFirstKeyPressed <= timeLimit)
            {
                firstKeyPressed = false;
                return true;
            }
            else
            {
                firstKeyPressed = false;
                return false;
            }
        }

        return false;
    }
}