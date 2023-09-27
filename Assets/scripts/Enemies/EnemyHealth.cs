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
    private float currentHealth;
    private int facingDirection;
    private Color originalColor;

    private void Awake()
    {
        enemyCol = GetComponent<BoxCollider2D>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();   
    }

    void Start()
    {
        originalColor = enemySR.color;
    }

    void Update()
    {
        if (transform.localScale.x > 0) facingDirection = 1;
        else if (transform.localScale.x < 0) facingDirection = -1;
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
