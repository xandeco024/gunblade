using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private HUDManager hudManager;
    [SerializeField] private PausePanel pausePanel;


    private void Awake()
    {

    }

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnLoad;
        gameManagerScript = GameObject.FindObjectOfType<GameManagerScript>();
    }

    void OnLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") Destroy(gameObject);

        hudManager.Retry();
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoad;
    }
}
