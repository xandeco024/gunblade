using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{

    //componentes
    [Header("Components")]
    private GameManagerScript gameManager;
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;

    [SerializeField] GameObject[] Cards ;

    public static int NumberOfCoins;
    public TextMeshProUGUI coinsText;

    private bool canMove = true;
    private bool isDead = false;

    private float currentHealth;

    private void Awake()
    {
        NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);

        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Start()
    {
    }

    void Update()
    {
        currentHealth = playerCombat.GetHealth();

        if (!isDead) 
        {
            if (currentHealth == 60) 
            {
                Destroy(Cards[4].gameObject);
            }
            if (currentHealth == 40)
            {
                Destroy(Cards[3].gameObject);
            }
            if (currentHealth == 20)
            {
                Destroy(Cards[2].gameObject);
            }
            if (currentHealth == 0)
            {
                Destroy(Cards[1].gameObject); 
                Destroy(Cards[0].gameObject);
            }
   
            if (currentHealth <= 0)
            {
                isDead = true;
            }
            if (isDead)
            {
                gameManager.gameOver();
            }
                coinsText.text = NumberOfCoins.ToString();
        }
    }
    void FixedUpdate()
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