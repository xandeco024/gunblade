using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D bulletRB;
    private CapsuleCollider2D bulletCol;

    [Header("Stats")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float bulletLifetime;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>(); 
        bulletCol = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log("penis");

            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("bateu no inimigo");
                DestroyBullet();
            }

            else if(collision.gameObject.layer == groundLayer)
            {
                Debug.Log("bateu no terreno");
                DestroyBullet();
            }
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
