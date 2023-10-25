using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    //componentes
    [Header("Components")]
    private GameManagerScript gameManagerScript;
    private PlayerCombat playerCombat;

    //Lista de Cartas

    [Header("Hud Elements")]
    [SerializeField] private GameObject[] cards;
    [SerializeField] private GameObject[] damageCards;
    [SerializeField] private GameObject[] AmmoUI;
    [SerializeField] private GameObject playerImage;

    [SerializeField] private TextMeshProUGUI coinsText;

    private float currentHealth;
    //private int currentAmmo;
    private int coins;

    private void Awake()
    {
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
        playerCombat = GameObject.FindObjectOfType<PlayerCombat>();

        int NumberOfCoins = PlayerPrefs.GetInt("NumberOfCoins", 0);
    }

    void Start()
    {

    }

    void Update()
    {
        currentHealth = gameManagerScript.PlayerHealth;
        //currentAmmo = gameManagerScript.PlayerAmmo;
        coins = gameManagerScript.PlayerBalance;

        coinsText.text = coins.ToString();
        CardHandler();
        //AmmoCheck();
    }

    void CardHandler()
    {
         if (currentHealth <= 0)
        {
            cards[0].SetActive(false);
            damageCards[0].SetActive(true);
        }

        else if (currentHealth <= 10)
        {
            cards[1].SetActive(false);
            damageCards[1].SetActive(true);
        }

        else if (currentHealth <= 20)
        {
            cards[2].SetActive(false);
            damageCards[2].SetActive(true);
        }

        else if (currentHealth <= 30)
        {
            cards[3].SetActive(false);
            damageCards[3].SetActive(true);
        }
    }

    public void Retry()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                cards[i].SetActive(true);
            }
        }

        for (int i = 0;i < damageCards.Length; i++)
        {
            if (damageCards[i] != null)
            {
                damageCards[i].SetActive(false);
            }
        }
    }

    /*void AmmoCheck() 
    {
        if (currentAmmo == 1) 
        {
            AmmoUI[1].SetActive(false);
        }

        else if (currentAmmo == 0) 
        {
            AmmoUI[0].SetActive(false);
        }
    }*/
}
