using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject input;
    public GameObject panel;
    public static float fadeTimer = 0f;
    
    public void Startgame()
    {
        string inputStr = input.GetComponent<TMP_InputField>().text;
        bool canStart = false;
        foreach(string s in Battle.availableCode)
        {
            if(inputStr.Equals(s)) { canStart = true; break; }
        }
        if(canStart)
        {
            Battle.code = inputStr;
            fadeTimer = 1.5f;
            panel.SetActive(true);
        }
        else
        {
            input.GetComponent<TMP_InputField>().text = "잘못된 코드입니다.";
        }
    }

    void Update()
    {
        if (gameObject.name.Equals("Panel"))
        {
            if(fadeTimer > 0f)
            { 
                fadeTimer -= Time.deltaTime;
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1 - fadeTimer / 1.5f);
            } else
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
