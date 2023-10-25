using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour {

        [SerializeField] private PlayerController playerController;
        
        /*  isso foi parte da tentativa
         * [SerializeField] private PlayerMovement playerMovement;
         
            private int facingDirection = -1;
         */
        
        private GameObject wayPoint;
        private Vector3 wayPointPos;
		[SerializeField] private float Distance;
		[SerializeField] private float atkDist = 3;
        
        [SerializeField] private float speed = 5.0f;
        [SerializeField] private int  followEnemyDamage = 2;
        
        


        void Start()
        {
            playerController = GameObject.FindObjectOfType<PlayerController>();

            /*playerMovement = GameObject.FindObjectOfType<PlayerMovement>();*/
            
            //At the start of the game, the zombies will find the gameobject called wayPoint.
            wayPoint = GameObject.Find("wayPoint");
        }
    
        void Update()
        {
            float plyDist = Vector2.Distance(transform.position, playerController.transform.position);
            
            //Animation();

            if(plyDist < Distance && plyDist > atkDist)
            {

                transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, speed * Time.deltaTime);
                //Animation();
            }

            if(plyDist < atkDist)
            {
                //Adaptado do Codigo de DeadZone mais uma vez
                playerController.GetComponent<PlayerCombat>().TakeDamage(followEnemyDamage, false);
            }
        }

		void OnDrawGizmosSelected()
        {
        
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, Distance);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, atkDist);
        }
    /* Tentei Fazer um flip para o Morcego
        void Flip()
        {
            facingDirection = -facingDirection;
            Vector3 enemyScale = transform.localScale;
            enemyScale.x *= 1;
            transform.localScale = enemyScale;
        }
        void CheckPlayerMov() 
        {
            if (playerMovement.horizontalInput > 0 && facingDirection == 1)
            {
                Flip();
            }

            else if (playerMovement.horizontalInput < 0 && facingDirection == -1) 
            {
                Flip(); 
            }
        }
    */

    //Chamar as Animacoes do Inimigo
}