using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject input;
    
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
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            input.GetComponent<TMP_InputField>().text = "잘못된 코드입니다.";
        }
    }
}
