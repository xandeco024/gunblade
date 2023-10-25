using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Resume()
    {
        if (Time.timeScale <= 0) Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        if (Time.timeScale <= 0) Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
