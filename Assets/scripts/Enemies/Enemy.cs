using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 3;
    int currentHealth;

    public float speed;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool facingRight = true;

    public PlayerController player;

    void Start()
    {
        currentHealth = MaxHealth;
        player = FindObjectOfType<PlayerController>();
    
    }
    public void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);

        if (ground == false) 
        {
            speed *= -1;
        }
        if (speed > 0 && !facingRight)
        {
            flip();
        }
        else if (speed < 0 && facingRight) 
        {
            flip();
        }
    }

    void flip() 
    {
        facingRight = !facingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }
    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    void Die() 
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
        }
    }
}
