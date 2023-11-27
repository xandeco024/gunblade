using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public bool IsPauseOn = false;

    [SerializeField] private GameObject panel;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IsPauseOn = true;
            panel.gameObject.SetActive(true);
        }
    }

    public void Resume()
    {
        IsPauseOn = false;
        if (Time.timeScale <= 0) Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        if (Time.timeScale <= 0) Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
