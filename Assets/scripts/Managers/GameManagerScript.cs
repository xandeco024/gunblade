using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;

    
    public GameObject player;
    public Vector2 RespawnPoint;


    void Start()
    {
         
    }

    void Update()
    {
        
    }
    public void gameOver() 
    {
        RespawnPoint = new Vector2 (player.transform.position.x, player.transform.position.y);
        gameOverUI.SetActive(true);
        
    }
    public void Reload() 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        
    }
    public void SceneChanger(string scenename) 
    {
        SceneManager.LoadScene(scenename);
    }
}
