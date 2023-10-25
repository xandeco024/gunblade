using System;
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

    // [Header("State Actions")]
    private Action startRunning;
    private Action stopRunning;
    private Action jump;
    private Action land;

    void Awake()
    {
        startRunning = () => HandleAnimation("Run", true);
        stopRunning = () => HandleAnimation("Run", false);
        jump = () => HandleAnimation("Jump", true);
        land = () => HandleAnimation("Land", true);

        PlayerMovement.OnStartRunning += startRunning;
        PlayerMovement.OnStopRunning += stopRunning;
        PlayerMovement.OnJump += jump;
        PlayerMovement.OnLand += land;
    }

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerController = GetComponent<PlayerController>();
    }

    private void HandleAnimation(string animation, bool state)
    {
        switch (animation)
        {
            case "Run":
                if (state) playerAnimator.SetTrigger("OnStartRunning");
                else playerAnimator.SetTrigger("OnStopRunning");
                break;

            case "Jump":
                if (state) playerAnimator.SetTrigger("OnJump");
                break;

            case "Land":
                if (state) playerAnimator.SetTrigger("OnLand");
                break;

            default: break;
        }
    }

    void OnDestroy()
    {
        PlayerMovement.OnStartRunning -= startRunning;
        PlayerMovement.OnStopRunning -= stopRunning;
        PlayerMovement.OnJump -= jump;
        PlayerMovement.OnLand -= land;
    }
}