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
    public static float fpsTimer = 0f;
    public static float fpsSum = 0f;
    public static int fpsCount = 0;
    public static string preCode = "placeholder";

    void Start()
    {
        Application.targetFrameRate = Battle.targetFrameRate;
        try
        {
            if (!preCode.Equals("placeholder")) input.GetComponent<TMP_InputField>().text = preCode;
        } catch (UnassignedReferenceException) { }

        if (Battle.doFastDamaging) GameObject.Find("FastButton").GetComponentInChildren<TMP_Text>().text = "��Ʋ �ӵ� : ����";
        else GameObject.Find("FastButton").GetComponentInChildren<TMP_Text>().text = "��Ʋ �ӵ� : ����";
    }
    
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
            preCode = inputStr;
            fadeTimer = 1.5f;
            panel.SetActive(true);
        }
        else
        {
            input.GetComponent<TMP_InputField>().text = "�߸��� �ڵ��Դϴ�.";
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

        if (gameObject.name.Equals("FpsTest"))
        {
            Battle.randomRefreshValue = Random.Range(float.MinValue, float.MaxValue);
            fpsTimer += Time.deltaTime;

            if (fpsTimer > 0.5f)
            {
                string fpsStatus;
                float fps = 1 / Time.deltaTime;
                fpsSum += fps;
                fpsCount++;
                if (fpsSum/fpsCount >= 50) fpsStatus = "(����)";
                else fpsStatus = "(������)";
                gameObject.GetComponent<TMP_Text>().text = $"{(int)fps} {fpsStatus}";
                fpsTimer = 0f;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
