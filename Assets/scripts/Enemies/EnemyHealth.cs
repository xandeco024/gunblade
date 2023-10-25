using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Components")]
    private BoxCollider2D enemyCol;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;

    [Header("Enemy Stats")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float knockBackForce;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private int coinAmount;
    private float currentHealth;
    private bool isDead = false;
    private int facingDirection;
    private Color originalColor;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject explosionParticle;

    private void Awake()
    {
        enemyCol = GetComponent<BoxCollider2D>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();   
    }

    void Start()
    {
        originalColor = enemySR.color;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection = -1;

        if(currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        GameObject blood = Instantiate(explosionParticle, transform.position, Quaternion.identity);

        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(float damage, bool knockBack)
    {
        currentHealth -= damage;
        StartCoroutine(Flash());
        if (knockBack) StartCoroutine(KnockBack());
    }

    IEnumerator Flash()
    {
        Color flashColor = new Color(255, 0, 0);

        enemySR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        enemySR.color = originalColor;
        yield return new WaitForSeconds(.1f);
        enemySR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        enemySR.color = originalColor;
        yield return new WaitForSeconds(.1f);
        enemySR.color = flashColor;
        yield return new WaitForSeconds(.1f);
        enemySR.color = originalColor;
    }

    IEnumerator KnockBack()
    {
        enemyRB.AddForce(new Vector2(-facingDirection, 1) * knockBackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockBackDuration);
    }
}
