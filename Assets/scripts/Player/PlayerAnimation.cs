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
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isAttacking;
    private bool isTakingDamage;
    private bool isShooting;
    private bool isDying;

    // [Header("State Actions")]

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UpdateStates();

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

        playerAnimator.SetFloat("YSpeed", playerController.playerMovement.YSpeed);

    }

    private void UpdateStates()
    {
        isRunning = playerController.playerMovement.IsRunning;
        isJumping = playerController.playerMovement.IsJumping;
        isWallJumping = playerController.playerMovement.IsWallJumping;
        isWallSliding = playerController.playerMovement.IsWallSliding;
        fallTrigger = playerController.playerMovement.FallTrigger;
        jumpTrigger = playerController.playerMovement.JumpTrigger;
    }
}