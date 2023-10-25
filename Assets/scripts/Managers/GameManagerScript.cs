using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerCombat playerCombat;

    [Header("PlayerStats")]
    private int playerHealth;
    public int PlayerHealth { get { return playerHealth; }}

    private int retryCost = 0;
    private int playerBalance = 1500;
    public int PlayerBalance { get { return playerBalance; } set { if (value <= 0) { playerBalance = 0; } else playerBalance = value; } }

    private Vector2 fallPoint = new Vector2(0, 0);
    public Vector2 FallPoint { get { return fallPoint; } set { fallPoint = value; } }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerCombat = playerController.gameObject.GetComponent<PlayerCombat>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") Destroy(gameObject);
        else
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }

    void Update()
    {
        //if (playerController.isPlayerDead())
        //{
        //    GameOverHandler(true, retryCost, playerBalance);
        //}
        //else GameOverHandler(false, retryCost, playerBalance);

        playerHealth = playerCombat.GetHealth();

        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerBalance = PlayerBalance + 100;
            Debug.Log("cu");
        }
    }
    /*public void GameOverHandler(bool dead,float cost, float balance)
    {
        if (dead)
        {
            if (!gameOverPanel.gameObject.activeSelf)
            {
                gameOverPanel.gameObject.SetActive(true);
            }

            if (retryCost > playerBalance)
            {
                gameOverPanel.OverCanvas(cost, balance);
            }
            else gameOverPanel.RetryCanvas(cost, balance);
        }

        else gameOverPanel.gameObject.SetActive(false);
    }*/
    public void Retry()
    {
        playerBalance -= retryCost;
        retryCost += 100;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}