using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //componentes
    [Header("Components")]
    private GameManagerScript gameManager;
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private Animator cardsAnimator;

    //Lista de Cartas
    [SerializeField]private GameObject[] cards;
    [SerializeField]private GameObject[] damageCards;


    private bool canMove = true;
    private bool isDead = false;

    private float currentHealth;


    private void Awake()
    {
        int NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);

        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
        cardsAnimator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {


    }

    public bool isPlayerDead()
    {
        return isDead;
    }

    public bool canPlayerMove()
    {
        return canMove;
    }

    public void setPlayerCanMove(bool move)
    {
        canMove = move;
    }

}