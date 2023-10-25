using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button retryBtn, quitBtn, settingsBtn;
    [SerializeField] private TextMeshProUGUI overText;

    [Header("Balance")]
    [SerializeField] private TextMeshProUGUI balanceValueText;
    [SerializeField] Color balanceValueColor1, balanceValueColor2;
    [SerializeField] private float balanceInterpolateTime;

    [Header("Cost")]
    [SerializeField] private TextMeshProUGUI costValueText;
    [SerializeField] Color costValueColor1, costValueColor2;
    [SerializeField] private float costInterpolateTime;
     
    float startTime = 0;

    private void Start()
    {
        startTime = Time.time;  
    }

    private void Update()
    {
    }
    public void OverCanvas(float cost, float balance)
    {
        if (!overText.gameObject.activeSelf) overText.gameObject.SetActive(true);
        if (retryBtn.interactable) retryBtn.interactable = false;
        costValueText.SetText("$ " + cost.ToString());
        costValueText.color = InterpolateColor(costValueColor1, costValueColor2, costInterpolateTime);
        balanceValueText.SetText("$ " + balance.ToString());
        balanceValueText.color = InterpolateColor(costValueColor1, costValueColor2, balanceInterpolateTime);
    }

    public void RetryCanvas(float cost, float balance)
    {
        if(overText.gameObject.activeSelf) overText.gameObject.SetActive(false);
        if (!retryBtn.interactable) retryBtn.interactable = true;
        costValueText.SetText("$ " + cost.ToString());
        costValueText.color = InterpolateColor(costValueColor1, costValueColor2, costInterpolateTime);
        balanceValueText.SetText("$ " + balance.ToString());
        balanceValueText.color = InterpolateColor(balanceValueColor1, balanceValueColor2, balanceInterpolateTime);
    }

    Color InterpolateColor(Color darkColor, Color brightColor, float interpolateTime)
    {
        float r = Mathf.PingPong(Time.time - startTime, interpolateTime);
        float rValue = Mathf.Lerp(darkColor.r, brightColor.r, r);

        float g = Mathf.PingPong(Time.time - startTime, interpolateTime);
        float gValue = Mathf.Lerp(darkColor.g, brightColor.g, g);

        float b = Mathf.PingPong(Time.time - startTime, interpolateTime);
        float bValue = Mathf.Lerp(darkColor.b, brightColor.b, b);

        Color color = new Color(rValue, gValue, bValue, 1);

        return color;
    }
}
