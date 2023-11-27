using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [Header("Components")]
    private Animator playerAnimator;
    private PlayerController playerController;

    [Header("State")]
    private bool isRunning;
    private bool isJumping;
    private bool fallTrigger;
    private bool jumpTrigger;
    private bool attackTrigger;
    private bool shootTrigger;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isAttacking;
    private bool isTakingDamage;
    private bool isShooting;
    private bool isDying;

    [Header("State Triggers")]
    private bool attackTriggered = false;
    private bool shotTriggered =  false;

    // [Header("State Actions")]

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        /*UpdateStates();

        if(isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else 
        {
            playerAnimator.SetBool("isRunning", false);
        }

        if(isWallSliding)
        {
            playerAnimator.SetBool("isWallSliding", true);
        }
        else
        {
            playerAnimator.SetBool("isWallSliding", false);
        }

        if (jumpTrigger) playerAnimator.SetTrigger("jumpTrigger");
        if (fallTrigger) playerAnimator.SetTrigger("fallTrigger");
        if(attackTrigger) playerAnimator.SetTrigger("attackTrigger");
        if(shootTrigger) playerAnimator.SetTrigger("shootTrigger");
        
        playerAnimator.SetFloat("YSpeed", playerController.playerMovement.YSpeed);
       
        */

        playerAnimator.SetBool("Grounded", playerController.playerMovement.IsGrounded);
        playerAnimator.SetFloat("XSpeed", Mathf.Abs(playerController.playerMovement.XSpeed));
        playerAnimator.SetFloat("YSpeed", playerController.playerMovement.YSpeed);
        playerAnimator.SetBool("WallSliding", playerController.playerMovement.IsWallSliding);
        playerAnimator.SetFloat("Rotation", playerController.playerCombat.Rotation);

        if (playerController.playerCombat.IsAttacking)
        {
            if(!attackTriggered){
                attackTriggered = true;
                playerAnimator.SetTrigger("Attack");
            }   
            else{
                attackTriggered = false;
            }
        }

        if (playerController.playerCombat.IsShooting)
        {
            if(!shotTriggered){
                shotTriggered = true;
                playerAnimator.SetTrigger("Shoot");
            }   
            else{
                shotTriggered = false;
            }
        }
    }

    /*private void UpdateStates()
    {
        isRunning = playerController.playerMovement.IsRunning;
        isJumping = playerController.playerMovement.IsJumping;
        isWallJumping = playerController.playerMovement.IsWallJumping;
        isWallSliding = playerController.playerMovement.IsWallSliding;
        fallTrigger = playerController.playerMovement.FallTrigger;
        jumpTrigger = playerController.playerMovement.JumpTrigger;
        attackTrigger = playerController.playerCombat.AttackTrigger;
        shootTrigger = playerController.playerCombat.ShotTrigger;
    }*/
}