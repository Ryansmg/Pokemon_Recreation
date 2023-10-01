using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{

    public static Pokemon player;
    public static Pokemon enemy;

    public static string code;
    public static string[] availableCode =
    {
        "0881467" //피카츄
    };

    /// <summary>
    /// Do not change the content manually.
    /// use UIManager.msg
    /// </summary>
    public GameObject messagePanel;
    public GameObject userBlockPanel;

    //battle management
    public static bool isPlayerTurn;
    public static bool isFirstTurn;
    public static Skill usedPlayerSkill = skill_none;
    public static Skill usedSkill = skill_none;
    public static int skillEffect = effect_none;
    public bool damagePlayer;

    public const int effect_none = 0;
    public const int effect_small = 1;
    public const int effect_medium = 2;
    public const int effect_large = 3;

    /// <summary>
    /// Only changing to true is available.
    /// </summary>
    public static bool waiting; //for end of animation / action with UI
    /// <summary>
    /// Only changing to true is available.
    /// </summary>
    public static bool waitingEnded;
    public static int nextUIType; //used when UI/Animation ended

    //UI management
    public static int currentUIType;
    public static int currentAnimationType;
    public static int nextAnimationType;
    public static bool doFastDamaging = false;

    public const int UI_playAnimation = -2; //UI_none, plays animation
    public const int UI_none = -1; //없음
    public const int UI_introduction = 0; //야생의 ~가 나타났다!
    public const int UI_skillUsed = 6; //speed 판정 후 skillExplanation으로 이동
    public const int UI_skillExplanation = 1; //스킬 사용 설명
    public const int UI_itemExplanation = 2; //아이템 설명
    public const int UI_skillResult = 3; //효과는 ~
    public const int UI_gameOver = 4; //눈앞 깜깜
    public const int UI_run = 5; //무사히 도망쳤다
    public const int UI_win = 7;

    public const int Ani_none = -99;
    public const int Ani_damage = 100;
    public const int Ani_fadeOutToSelectScene = 101;

    public GameObject blackPanel;

    public static Skill heal = new(1000, -75, Skill.type_normal, "상처약");
    public static Skill skill_none = new(0, 0, Skill.type_normal, "없음");
    public static Skill hundKBolt = new(15, 90, Skill.type_electric, "10만볼트");
    public static Skill ironTail = new(15, 100, Skill.type_iron, "아이언테일");
    public static Skill fast = new(5, 80, Skill.type_normal, "신속");
    public static Skill lightning = new(10, 110, Skill.type_electric, "번개");
    public static Skill struggling = new(int.MaxValue, 10, Skill.type_normal, "발버둥");

    void Start()
    {
        waiting = false;
        waitingEnded = true;
        currentUIType = UI_none;
        currentAnimationType = Ani_none;
        nextUIType = UI_introduction;
        nextAnimationType = Ani_none;

        UIManager.msgPanel = messagePanel;

        player = new Pokemon("피카츄", Skill.type_electric, 350, 10, hundKBolt, lightning, ironTail, fast);
        Pokemon pikachu = new("야생 피카츄", Skill.type_electric, 350, 10, hundKBolt, lightning, ironTail, fast);
        if (code.Equals(availableCode[0]))
        {
            enemy = pikachu;
        }
    }

    void LateUpdate()
    {
        // if (waiting) Debug.Log("waiting");

        player.hpChanged = false;
        enemy.hpChanged = false;

        if (player.hp <= 0 && !waiting && currentUIType == UI_gameOver)
        {
            Debug.Log("Game Over");
        }

        if (waitingEnded)
        {
            Debug.Log($"UIType {currentUIType}->{nextUIType}");
            waiting = false;
            waitingEnded = false;
            currentUIType = nextUIType;
            if(currentUIType != UI_playAnimation)
                userBlockPanel.SetActive(false);
            switch (currentUIType)
            {
                case UI_none:
                    UIManager.msgPanel.SetActive(false);
                    goto endOfUIChange;

                case UI_introduction:
                    UIManager.msgType = UIManager.msg_default;
                    UIManager.msg = "야생의 피카츄가 나타났다!";
                    UIManager.msgPanel.SetActive(true);
                    waiting = true;
                    nextUIType = UI_none;
                    goto endOfUIChange;

                case UI_skillUsed:
                    if (player.speed >= enemy.speed) isPlayerTurn = true;
                    else isPlayerTurn = false;
                    if (usedPlayerSkill.skillName.Equals("상처약")) isPlayerTurn = true;
                    isFirstTurn = true;
                    nextUIType = UI_skillExplanation;
                    waitingEnded = true;
                    break;

                case UI_skillExplanation:
                    UIManager.msgType = UIManager.msg_default;

                    string pokemonName = isPlayerTurn ? player.name : enemy.name;

                    if (isPlayerTurn) usedSkill = usedPlayerSkill;
                    else
                    {
                        usedSkill = skill_none;
                        int ppLeft = 0;
                        foreach(Skill skill in enemy.skill)
                        {
                            ppLeft += skill.skillPP;
                        }
                        if (ppLeft <= 0) usedSkill = struggling;
                        else
                        {
                            while(usedSkill.skillPP <= 0)
                            {
                                usedSkill = enemy.skill[Random.Range(0, 4)];
                            }
                        }
                    }

                    usedSkill.skillPP--;

                    UIManager.msg = $"{pokemonName}의 \n{usedSkill.skillName}!";
                    if(usedSkill.skillName.Equals("상처약")) UIManager.msg = $"{pokemonName}에게 상처약을 사용했다!";
                    UIManager.msgPanel.SetActive(true);

                    nextAnimationType = Ani_damage;
                    nextUIType = UI_playAnimation;
                    waiting = true;
                    break;

                case UI_skillResult:
                    if (player.hp <= 0) { nextUIType = UI_gameOver; }
                    if (enemy.hp <= 0) { nextUIType = UI_win; }
                    else
                    {
                        if (isFirstTurn)
                        {
                            isFirstTurn = false;
                            isPlayerTurn = !isPlayerTurn;
                            nextUIType = UI_skillExplanation;
                        }
                        else
                        {
                            nextUIType = UI_none;
                        }
                    }
                    if (usedSkill.skillName.Equals("상처약")) waitingEnded = true;
                    else
                    {
                        UIManager.msg = "효과는 굉장했다!";
                        UIManager.msgPanel.SetActive(true);
                        UIManager.msgType = UIManager.msg_default;
                        waiting = true;
                    }
                    break;

                case UI_gameOver:
                    float testRand = Random.Range(128f, 16384f);
                    UIManager.msg = $"더 이상 싸울 수 있는 포켓몬이 없다!\n눈앞이 깜깜해졌다...\n{(int)testRand}원을 잃었다.";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true; break;

                case UI_win:
                    UIManager.msg = "승리했다!";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true;
                    break;

                case UI_run:
                    UIManager.msg = "무사히 도망쳤다.";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true; break;

                case UI_playAnimation:
                    UIManager.msgPanel.SetActive(false);
                    nextUIType = UI_none;
                    currentAnimationType = nextAnimationType;
                    nextAnimationType = Ani_none;
                    userBlockPanel.SetActive(true);
                    if (currentAnimationType == Ani_fadeOutToSelectScene)
                        blackPanel.SetActive(true);
                    if (currentAnimationType == Ani_damage)
                    {
                        damagePlayer = true;
                    }
                    waiting = true;
                    break;

                default:
                    UIManager.msgPanel.SetActive(false);
                    currentUIType = UI_none;
                    nextUIType = UI_none;
                    goto endOfUIChange;
            }
        }

    endOfUIChange:

        if (damagePlayer)
        {
            damagePlayer = false;
            if (!isPlayerTurn)
            {
                player.hpChanged = true;
                player.hp -= usedSkill.skillDamage;
            }
            else
            {
                if (usedSkill.skillName.Equals("상처약"))
                {
                    player.hpChanged = true;
                    player.hp -= heal.skillDamage;
                }
                else
                {
                    enemy.hpChanged = true;
                    enemy.hp -= usedSkill.skillDamage;
                }
            }
        }

        if (player.hp > player.maxHp) player.hp = player.maxHp;
        if (enemy.hp > enemy.maxHp) enemy.hp = enemy.maxHp;
    }
}