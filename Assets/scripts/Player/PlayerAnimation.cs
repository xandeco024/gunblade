using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    [Header("Components")]
    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerController playerController;

    [Header("State")]
    private bool isRunning;
    private bool isJumping;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool isAttacking;
    private bool isTakingDamage;
    private bool isShooting;
    private bool isDying;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        UpdateState();

        if (isRunning) playerAnimator.SetBool("isRunning", true);
    }

    void UpdateState()
    {
        isRunning = playerMovement.IsRunning();
        isJumping = playerMovement.IsJumping();
        isWallJumping = playerMovement.IsWallJumping();
        isWallSliding = playerMovement.IsWallSliding();
    }
}
