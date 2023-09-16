using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;

    public Transform AttackPoint;
    public LayerMask enemyLayers;


    public float attackRange = 0.5f;
    public int attackDamage = 1;

    public int combo;
    public bool attacking;
    void Update()
    {
        Combos_();
    }
    void Combos_()
    {
        if (Input.GetKeyDown(KeyCode.F) && !attacking)
        {
            attacking = true;
            anim.SetTrigger("" + combo);
        }


        Collider2D [] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies) 
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    public void Start_Combo() 
    {
        attacking = false;
        if (combo < 3) 
        {
            combo++;
        }
    
    }
    public void Finish_Ani() 
    {
        attacking = false;
        combo = 0;
    }
    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;

        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
        
    }
}
