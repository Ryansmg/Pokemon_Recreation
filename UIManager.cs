using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public string type;
    public int skillNumber;

    public static GameObject msgPanel;
    public static string msg = "";
    public static int msgType = 0;

    /// <summary>
    /// Display style preview text.
    /// </summary>
    public const int msg_plain = -1;
    /// <summary>
    /// > Display each line with angle brackets.
    /// </summary>
    public const int msg_default = 0;
    /// <summary>
    /// [Display_without_line_breaks.]
    /// </summary>
    public const int msg_system = 1;
    /// <summary>
    /// {error}: Display with 'error' text.
    /// </summary>
    public const int msg_error = 2;

    public static bool setSpeed = true;

    public static int damageFrameCount = 75;
    public static float fixedUnitWidth = 1.68f;
    const float damageBarMaxWidth = 1260f;
    float destinationWidth;
    float unitWidth;
    int damagedFrameCount = 0;
    float damageWaitTimer = 0f;
    static bool playerDamageAnimation = false;
    static bool enemyDamageAnimation = false;

    public static int fastDamageFrameCount = 20;

    public static float fadeOutTimer = 0;
    public static float fadeInTimer = 0;

    void Update()
    {

        switch (type.ToLower())
        {
            case "healbutton":
                if (Battle.heal.skillPP <= 0) gameObject.GetComponent<Button>().interactable = false;
                else gameObject.GetComponent<Button>().interactable = true;
                break;

            case "heal2button":
                if (Battle.heal2.skillPP <= 0) gameObject.GetComponent<Button>().interactable = false;
                else gameObject.GetComponent<Button>().interactable = true;
                break;

            case "healpp":
                gameObject.GetComponent<TMP_Text>().text = skillNumber switch
                {
                    0 => $"개수: {Battle.heal.skillPP}/{Battle.heal.skillMaxPP}",
                    1 => $"개수: {Battle.heal2.skillPP}/{Battle.heal2.skillMaxPP}",
                    _ => "Error"
                };
                break;

            case "skillbutton":
                if (Battle.player.skill[skillNumber].skillPP <= 0) gameObject.GetComponent<Button>().interactable = false;
                else gameObject.GetComponent<Button>().interactable = true;
                break;
            case "skilltype":
                string type = skillNumber switch
                {
                    0 => "물",
                    1 => "노말",
                    2 => "물",
                    3 => "악",
                    _ => "???"
                };
                gameObject.GetComponent<TMP_Text>().text = $"<{type}>\nPP: {Battle.player.skill[skillNumber].skillPP}/{Battle.player.skill[skillNumber].skillMaxPP}";
                
                break;

            case "msgpanel":
                SetMessage();
                break;

            case "enemyname":
                string typeStr = Battle.enemy.type switch
                {
                    Skill.type_electric => "전기",
                    Skill.type_fire => "불꽃",
                    Skill.type_dragon => "드래곤",
                    _ => "???"
                };
                gameObject.GetComponent<TMP_Text>().text = $"{Battle.enemy.name.Replace("야생 ", "")} <{typeStr}>";
                break;

            case "playerhp":
                float currentWidth = gameObject.GetComponent<RectTransform>().rect.width;

                if (Battle.player.hpChanged)
                {
                    playerDamageAnimation = true;
                    ConfigureDamageAnimation(currentWidth);
                    Battle.currentAnimationType = Battle.Ani_damage;
                    damagedFrameCount = 0;
                    Battle.waiting = true;
                }

                if (damagedFrameCount < damageFrameCount && playerDamageAnimation)
                {
                    if (currentWidth - unitWidth > damageBarMaxWidth)
                        gameObject.GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, damageBarMaxWidth);
                    else gameObject.GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth - unitWidth);

                    damagedFrameCount++;
                }
                else
                {
                    if (Battle.currentAnimationType == Battle.Ani_damage && playerDamageAnimation)
                    {
                        if (damageWaitTimer > 0) damageWaitTimer -= Time.deltaTime;
                        else
                        {
                            Battle.waitingEnded = true;
                            Battle.nextUIType = Battle.UI_skillResult;
                            Battle.currentAnimationType = Battle.Ani_none;
                            playerDamageAnimation = false;
                        }
                    }
                }

                break;



            case "enemyhp":
                float currentWidth2 = gameObject.GetComponent<RectTransform>().rect.width;

                if (Battle.enemy.hpChanged)
                {
                    enemyDamageAnimation = true;
                    ConfigureDamageAnimation(currentWidth2);
                    Battle.currentAnimationType = Battle.Ani_damage;
                    damagedFrameCount = 0;
                    Battle.waiting = true;
                }

                if (damagedFrameCount < damageFrameCount && enemyDamageAnimation)
                {
                    if (currentWidth2 - unitWidth > damageBarMaxWidth)
                        gameObject.GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, damageBarMaxWidth);
                    else gameObject.GetComponent<RectTransform>()
                            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentWidth2 - unitWidth);

                    damagedFrameCount++;
                }
                else
                {
                    
                    if (Battle.currentAnimationType == Battle.Ani_damage && enemyDamageAnimation)
                    {
                        if (damageWaitTimer > 0) damageWaitTimer -= Time.deltaTime;
                        else
                        {
                            Battle.waitingEnded = true;
                            enemyDamageAnimation = false;
                            Battle.nextUIType = Battle.UI_skillResult;
                            Battle.currentAnimationType = Battle.Ani_none;
                        }
                    }
                    
                }

                break;

            case "blackpanel":
                if(Battle.currentAnimationType == Battle.Ani_fadeOutToSelectScene)
                {
                    if(fadeOutTimer <= 0)
                    {
                        /* Animation start */
                        gameObject.SetActive(true);
                        fadeOutTimer = 1.5f + Time.deltaTime;
                    }

                    fadeOutTimer -= Time.deltaTime;
                    gameObject.transform.GetComponent<Image>().color = new Color(0, 0, 0, 1f-fadeOutTimer/1.5f);

                    if(fadeOutTimer <= 0)
                    {
                        /* Animation end */
                        Battle.currentAnimationType = Battle.Ani_none;
                        Battle.waitingEnded = true;
                        SceneManager.LoadScene("SelectScene");
                    }
                }
                if (Battle.currentAnimationType == Battle.Ani_fadeIn)
                {
                    if (fadeInTimer <= 0)
                    {
                        /* Animation start */
                        gameObject.SetActive(true);
                        fadeInTimer = 2f + Time.deltaTime;
                    }

                    fadeInTimer -= Time.deltaTime;
                    gameObject.transform.GetComponent<Image>().color = new Color(0, 0, 0, fadeInTimer / 1.5f);

                    if (fadeInTimer <= 0)
                    {
                        /* Animation end */
                        Battle.currentAnimationType = Battle.Ani_none;
                        Battle.waitingEnded = true;
                        gameObject.SetActive(false);
                        Battle.nextUIType = Battle.UI_introduction;
                    }
                }
                break;

            default:
                Debug.LogError("Wrong UIManager type.");
                break;
        }
    }

    void ConfigureDamageAnimation(float currentWidth)
    {
        if(playerDamageAnimation) destinationWidth = 1260f * Battle.player.hp / Battle.player.maxHp;
        else destinationWidth = 1260f * Battle.enemy.hp / Battle.enemy.maxHp;

        if (!setSpeed)
        {
            unitWidth = (currentWidth - destinationWidth) / damageFrameCount;
            if (Battle.doFastDamaging) { damageFrameCount = fastDamageFrameCount; }
            else damageFrameCount = 75;
        }
        else
        {
            if (Battle.doFastDamaging) { fixedUnitWidth = 10f; }
            else fixedUnitWidth = 1.68f;
            if (currentWidth - destinationWidth > 0) unitWidth = fixedUnitWidth;
            else unitWidth = -1 * fixedUnitWidth;
            damageFrameCount = (int)Mathf.Ceil((currentWidth - destinationWidth) / unitWidth);
        }

        if (Battle.doFastDamaging) damageWaitTimer = 0.2f;
        else damageWaitTimer = 0.4f;
    }

    void SetMessage()
    {
        if (msg.StartsWith("$Red$"))
            gameObject.GetComponent<TMP_Text>().color = new Color(255 / 255, 105f / 255f, 105f / 255f);
        else gameObject.GetComponent<TMP_Text>().color = Color.white;

        switch (msgType)
        {
            case msg_plain:
                gameObject.GetComponent<TMP_Text>().text = msg.Replace("$Red$", "");
                break;
            case msg_default:
                /*string s = "";
                string[] lines = msg.Replace("\r", "").Split('\n');
                foreach(string line in lines) s += "> " + line + "\n";
                gameObject.GetComponent<TMP_Text>().text = s;
                break;*/
            default:
                gameObject.GetComponent<TMP_Text>().text = msg.Replace("$Red$", "");
                break;
        }
    }
}
