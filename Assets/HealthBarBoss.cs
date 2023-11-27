using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    [SerializeField] private Slider sliderBoss;

    public void SetHealth(float health, float maxHealth) 
    {
        sliderBoss.value = health;
        sliderBoss.maxValue = maxHealth;        
    }

    void Update() 
    {
        if (sliderBoss.value == 0)
        {
            sliderBoss.gameObject.SetActive(false);
        }
    }
}
