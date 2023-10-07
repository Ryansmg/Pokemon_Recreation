using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{

    public static Pokemon player;
    public static Pokemon enemy;

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
    public static Skill usedEnemySkill = skill_none;
    public static Skill usedSkill = skill_none;
    public static int skillEffect = effect_none;
    public bool damagePlayer;
    public bool showCriticalMsg = false;

    public const int effect_missed = -1;
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
    public static bool doFastDamaging = true;

    public const int UI_playAnimation = -2; //UI_none, plays animation
    public const int UI_none = -1; //����
    public const int UI_introduction = 0; //�߻��� ~�� ��Ÿ����!
    public const int UI_skillUsed = 6; //speed ���� �� skillExplanation���� �̵�
    public const int UI_skillExplanation = 1; //��ų ��� ����
    public const int UI_itemExplanation = 2; //������ ����
    public const int UI_skillResult = 3; //ȿ���� ~
    public const int UI_gameOver = 4; //���� ����
    public const int UI_run = 5; //������ �����ƴ�
    public const int UI_win = 7;

    public const int Ani_none = -99;
    public const int Ani_damage = 100;
    public const int Ani_fadeOutToSelectScene = 101;
    public const int Ani_fadeIn = 102;

    public static string code = "9";
    public static string[] availableCode = { "1467", "39164", "485027", "V2xoS2VXSXpTV2c9" };

    public GameObject blackPanel;

    public static Skill tidePlayer = new(15, 90, Skill.type_water, 100, "�ĵ�Ÿ��");
    public static Skill quickAttackPlayer = new(30, 60, Skill.type_normal, 100, "������ȭ");
    public static Skill hydroPumpPlayer = new(5, 120, Skill.type_water, 80, "���̵������");
    public static Skill suprizePlayer = new(15, 70, Skill.type_dark, 100, "��¦����"); //�ڼӺ��� ���� (�������� ��Ÿ��)

    public static Skill heal = new(5, -100, Skill.type_normal, 100, "��ó��");
    public static Skill heal2 = new(2, -300, Skill.type_normal, 100, "��ó��");
    public static Skill skill_none = new(0, 0, Skill.type_normal, 100, "����");
    public static Skill struggling = new(int.MaxValue, 50, Skill.type_normal, 100, "�߹���");

    //��ī��
    public static Skill hundKBolt = new(15, 90, Skill.type_electric, 100, "10����Ʈ");
    public static Skill ironTail = new(15, 100, Skill.type_iron, 75, "���̾�����");
    public static Skill quickAttack = new(30, 60, Skill.type_normal, 100, "������ȭ");
    public static Skill lightning = new(10, 110, Skill.type_electric, 70, "����");

    //���ڸ�
    public static Skill fireBS = new(15, 90, Skill.type_fire, 100, "ȭ�����");
    //airSlash
    public static Skill dragonAir = new(10, 85, Skill.type_dragon, 100, "�����ĵ�");
    public static Skill solarBeam = new(10, 120, Skill.type_grass, 100, "�ֶ��");

    //������
    public static Skill dragonAscent = new(5, 120, Skill.type_dragon, 100, "ȭ������");
    public static Skill fast = new(5, 80, Skill.type_normal, 100, "�ż�");
    //lightning
    //public static Skill iceBeam = new(10, 90, Skill.type_ice, 100, "�õ���");
    public static Skill airSlash = new(15, 75, Skill.type_normal, 95, "�������");

    public GameObject pikachuO, charizardO, rayquazaO;

    public static float randomRefreshValue;
    public static Skill test = new(1000, 1, Skill.type_ice, 100, "");
    public static Skill test2 = new(1000, 1000, Skill.type_normal, 0, "\n");
    void Start()
    {
        Application.targetFrameRate = 60;

        waiting = false;
        waitingEnded = true;
        currentUIType = UI_none;
        currentAnimationType = Ani_none;
        nextUIType = UI_playAnimation;
        nextAnimationType = Ani_fadeIn;

        Skill.ResetSkills();

        UIManager.msgPanel = messagePanel;

        player = new Pokemon("��������", Skill.type_water, 720, 122, tidePlayer, quickAttackPlayer, hydroPumpPlayer, suprizePlayer);
        Pokemon pikachu = new("�߻� ��ī��", Skill.type_electric, 350, 90, hundKBolt, lightning, ironTail, quickAttack);
        Pokemon charizard = new("�߻� ���ڸ�", Skill.type_fire, 780, 100, fireBS, airSlash, dragonAir, solarBeam);
        Pokemon rayquaza = new("�߻� ������", Skill.type_dragon, 1050, 95, dragonAscent, fast, lightning, airSlash);
        Pokemon testPokemon = new("�߻� ???", Skill.type_water, 7200, int.MinValue, heal, heal2, test, test2);

        if (code.Equals(availableCode[0])) { enemy = pikachu; pikachuO.SetActive(true); }
        else if (code.Equals(availableCode[1])) { enemy = charizard; charizardO.SetActive(true); }
        else if (code.Equals(availableCode[2])) { enemy = rayquaza; rayquazaO.SetActive(true); }
        else if (code.Equals(availableCode[3])) { enemy = testPokemon; }
        else { enemy = testPokemon; SceneManager.LoadScene("SelectScene"); }
    }

    void LateUpdate()
    {
        // if (waiting) Debug.Log("waiting");

        randomRefreshValue = Random.Range(float.MinValue, float.MaxValue);

        player.hpChanged = false;
        enemy.hpChanged = false;

        if (player.hp <= 0 && !waiting && currentUIType == UI_gameOver)
        {
            Debug.Log("Game Over");
        }

        if (waitingEnded)
        {
            //Debug.Log($"UIType {currentUIType}->{nextUIType}");
            waiting = false;
            waitingEnded = false;
            currentUIType = nextUIType;
            if(currentUIType != UI_playAnimation)
                userBlockPanel.SetActive(false);
            switch (currentUIType)
            {
                case UI_none:
                    UIManager.msgPanel.GetComponentInChildren<TMP_Text>().text = "";
                    UIManager.msgPanel.SetActive(false);
                    goto endOfUIChange;

                case UI_introduction:
                    UIManager.msgType = UIManager.msg_default;
                    UIManager.msg = $"�߻��� {enemy.name.Replace("�߻� ","")}��(��) ��Ÿ����!";
                    UIManager.msgPanel.SetActive(true);
                    blackPanel.SetActive(false);
                    waiting = true;
                    nextUIType = UI_none;
                    goto endOfUIChange;

                case UI_skillUsed:
                    // �� ��ų ����
                    usedEnemySkill = skill_none;
                    int ppLeft = 0;
                    foreach (Skill skill in enemy.skill)
                    {
                        ppLeft += skill.skillPP;
                    }
                    if (ppLeft <= 0) usedEnemySkill = struggling;
                    else
                    {
                        while (usedEnemySkill.skillPP <= 0)
                        {
                            usedEnemySkill = enemy.skill[Random.Range(0, 4)];
                        }
                    }
                    //�������
                    int playerSpeed = player.speed;
                    int enemySpeed = enemy.speed;
                    if (usedPlayerSkill.skillName.Equals("��ó��")) playerSpeed = int.MaxValue;
                    if (usedPlayerSkill.skillName.Equals("������ȭ")) playerSpeed += 10000;
                    if (usedEnemySkill.skillName.Equals("������ȭ") || usedEnemySkill.skillName.Equals("�ż�")) enemySpeed += 10000;
                    if (playerSpeed >= enemySpeed) isPlayerTurn = true;
                    else isPlayerTurn = false;

                    isFirstTurn = true;
                    nextUIType = UI_skillExplanation;
                    waitingEnded = true;
                    break;

                case UI_skillExplanation:
                    UIManager.msgType = UIManager.msg_default;

                    string pokemonName = isPlayerTurn ? player.name : enemy.name;

                    if (isPlayerTurn) usedSkill = usedPlayerSkill;
                    else usedSkill = usedEnemySkill;

                    usedSkill.skillPP--;
                    struggling.skillPP = int.MaxValue;

                    UIManager.msg = $"{pokemonName}�� \n{usedSkill.skillName}!";
                    if(usedSkill.skillName.Equals("��ó��") && usedSkill.skillMaxPP == heal.skillMaxPP) UIManager.msg = $"{pokemonName}���� ��ó���� ����ߴ�!";
                    if (usedSkill.skillName.Equals("��ó��") && usedSkill.skillMaxPP == heal2.skillMaxPP) UIManager.msg = $"{pokemonName}���� ������ó���� ����ߴ�!";
                    UIManager.msgPanel.SetActive(true);

                    nextAnimationType = Ani_damage;
                    nextUIType = UI_playAnimation;
                    waiting = true;
                    break;

                case UI_skillResult:
                    bool wasFirstTurn = false;
                    bool wasPlayerTurn = isPlayerTurn;
                    if (player.hp <= 0) { nextUIType = UI_gameOver; }
                    else if (enemy.hp <= 0) { nextUIType = UI_win; }
                    else
                    {
                        if (isFirstTurn)
                        {
                            wasFirstTurn = true;
                            isFirstTurn = false;
                            isPlayerTurn = !isPlayerTurn;
                            nextUIType = UI_skillExplanation;
                        }
                        else
                        {
                            nextUIType = UI_none;
                        }
                    }
                    if (showCriticalMsg)
                    {
                        showCriticalMsg = false;
                        UIManager.msg = "$Red$�޼ҿ� �¾Ҵ�!";
                        if (wasFirstTurn) isFirstTurn = true;
                        isPlayerTurn = !isPlayerTurn;

                        nextUIType = UI_skillResult;
                        UIManager.msgPanel.SetActive(true);
                        UIManager.msgType = UIManager.msg_default;
                        waiting = true;
                    }
                    else if (usedSkill.skillName.Equals("��ó��") || skillEffect == effect_medium) waitingEnded = true;
                    else
                    {
                        UIManager.msg = skillEffect switch
                        {
                            effect_missed => $"{(wasPlayerTurn ? enemy.name : player.name)}���Դ�\n���� �ʾҴ�!",
                            effect_none => "ȿ���� ����...",
                            effect_small => "ȿ���� ������ ���ϴ�...",
                            effect_large => "ȿ���� �����ߴ�!",
                            _ => "Error!"
                        };
                        UIManager.msgPanel.SetActive(true);
                        UIManager.msgType = UIManager.msg_default;
                        waiting = true;
                    }
                    break;

                case UI_gameOver:
                    float testRand = Random.Range(128f, 16384f);
                    UIManager.msg = $"�� �̻� �ο� �� �ִ� ���ϸ��� ����!\n������ ����������...\n{(int)testRand}���� �Ҿ���.";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true; break;

                case UI_win:
                    UIManager.msg = $"{enemy.name}��(��) ��������!";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true;
                    break;

                case UI_run:
                    UIManager.msg = "������ �����ƴ�.";
                    UIManager.msgPanel.SetActive(true);
                    UIManager.msgType = UIManager.msg_default;
                    nextUIType = UI_playAnimation;
                    nextAnimationType = Ani_fadeOutToSelectScene;
                    waiting = true; break;

                case UI_playAnimation:
                    UIManager.msgPanel.GetComponentInChildren<TMP_Text>().text = "";
                    UIManager.msgPanel.SetActive(false);
                    nextUIType = UI_none;
                    currentAnimationType = nextAnimationType;
                    nextAnimationType = Ani_none;
                    userBlockPanel.SetActive(true);

                    if (currentAnimationType == Ani_fadeOutToSelectScene || currentAnimationType == Ani_fadeIn)
                        blackPanel.SetActive(true);

                    if (currentAnimationType == Ani_damage)
                    {
                        damagePlayer = true;
                    }

                    waiting = true;
                    break;

                default:
                    UIManager.msgPanel.GetComponentInChildren<TMP_Text>().text = "";
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
            if (Random.Range(1, 101) <= usedSkill.skillAccuracyPercent) {
                // ���� ��
                float damageChange = 1f;

                Pokemon attacker, target;
                if (isPlayerTurn) { attacker = player; target = enemy; }
                else { attacker = enemy; target = player; }

                //�ڼӺ���
                if (attacker.type == usedSkill.skillType) damageChange *= 1.5f;
                if (attacker.name.Equals("��������") && usedSkill.skillName.Equals("��¦����")) damageChange *= 1.5f;

                //TODO �� ���

                if(target.type == Skill.type_electric)
                {
                    switch(usedSkill.skillType)
                    {
                        case Skill.type_iron:
                        case Skill.type_electric:
                            damageChange *= 0.5f;
                            skillEffect = effect_small;
                            break;
                        case Skill.type_ground:
                            damageChange *= 2f;
                            skillEffect = effect_large;
                            break;
                        default:
                            skillEffect = effect_medium;
                            break;
                    }
                }

                if(target.type == Skill.type_water)
                {
                    switch (usedSkill.skillType)
                    {
                        case Skill.type_grass:
                        case Skill.type_electric:
                            damageChange *= 2f;
                            skillEffect = effect_large;
                            break;

                        case Skill.type_water:
                        case Skill.type_fire:
                        case Skill.type_iron:
                        case Skill.type_ice:
                            damageChange *= 0.5f;
                            skillEffect = effect_small;
                            break;

                        default:
                            skillEffect = effect_medium;
                            break;
                    }
                }

                if(target.name.Replace("�߻� ", "") == "��������")
                {
                    switch (usedSkill.skillType)
                    {
                        case Skill.type_grass:
                        case Skill.type_electric:
                        case Skill.type_bug:
                        case Skill.type_fairy:
                            damageChange *= 2f;
                            skillEffect = effect_large;
                            break;

                        case Skill.type_water:
                        case Skill.type_fire:
                        case Skill.type_iron:
                        case Skill.type_ice:
                        case Skill.type_dark:
                            damageChange *= 0.5f;
                            skillEffect = effect_small;
                            break;

                        default:
                            skillEffect = effect_medium;
                            break;
                    }
                }

                if(target.type == Skill.type_fire)
                {
                    switch (usedSkill.skillType)
                    {
                        case Skill.type_ground:
                        case Skill.type_water:
                        case Skill.type_rock:
                            damageChange *= 2f;
                            skillEffect = effect_large;
                            break;

                        case Skill.type_iron:
                        case Skill.type_ice:
                        case Skill.type_bug:
                        case Skill.type_fire:
                        case Skill.type_fairy:
                        case Skill.type_grass:
                            damageChange *= 0.5f;
                            skillEffect = effect_small;
                            break;

                        default:
                            skillEffect = effect_medium;
                            break;
                    }

                }

                if (target.type == Skill.type_dragon)
                {
                    switch (usedSkill.skillType)
                    {
                        case Skill.type_dragon:
                        case Skill.type_ice:
                        case Skill.type_fairy:
                            damageChange *= 2f;
                            skillEffect = effect_large;
                            break;

                        case Skill.type_water:
                        case Skill.type_fire:
                        case Skill.type_electric:
                        case Skill.type_grass:
                            damageChange *= 0.5f;
                            skillEffect = effect_small;
                            break;

                        default:
                            skillEffect = effect_medium;
                            break;
                    }
                }

                //end

                float criticalPos = 6.25f;
                if (usedSkill.skillName.Equals("��¦����")) criticalPos *= 2;
                float poss = Random.Range(0f, 100f);
                //Debug.Log(poss);

                if ( poss <= criticalPos && !usedSkill.skillName.Equals("��ó��")) { damageChange *= 2; showCriticalMsg = true; }
                else showCriticalMsg = false;

                if (usedSkill.skillName.Equals("��ó��")) damageChange = 1;

                if (!isPlayerTurn)
                {
                    player.hpChanged = true;
                    player.hp -= Mathf.RoundToInt(usedSkill.skillDamage * damageChange);
                }
                else
                {
                    if (usedSkill.skillName.Equals("��ó��"))
                    {
                        player.hpChanged = true;
                        player.hp -= usedSkill.skillDamage;
                    }
                    else
                    {
                        enemy.hpChanged = true;
                        enemy.hp -= Mathf.RoundToInt(usedSkill.skillDamage * damageChange);
                    }
                }
            } else
            {
                // ���� �ʾ��� ��

                skillEffect = effect_missed;
                if (!isPlayerTurn)
                {
                    player.hpChanged = true;
                }
                else
                {
                    enemy.hpChanged = true;
                }
            }
        }

        if (player.hp > player.maxHp) player.hp = player.maxHp;
        if (enemy.hp > enemy.maxHp) enemy.hp = enemy.maxHp;
    }
}