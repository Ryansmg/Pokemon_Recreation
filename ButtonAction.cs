using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public GameObject attackSelection;
    public TMP_Text attackInfoNonStatic;
    public GameObject attackInfoPanelNS;
    public static TMP_Text attackInfo;
    public static GameObject attackInfoPanel;

    public GameObject itemSelection;

    void Start()
    {
        attackInfo = attackInfoNonStatic;
        attackInfoPanel = attackInfoPanelNS;
    }

    public void SelectAttack()
    {
        int ppLeft = 0;
        foreach (Skill skill in Battle.player.skill)
        {
            ppLeft += skill.skillPP;
        }
        if (ppLeft <= 0)
        {
            Battle.nextUIType = Battle.UI_skillUsed;
            Battle.usedPlayerSkill = Battle.struggling;
            Battle.waitingEnded = true;
        }
        else
        {
            attackSelection.SetActive(true);
            attackInfoPanel.SetActive(true);
        }
    }

    public void Attack(int n)
    {
        if (Battle.player.skill[n].skillPP <= 0) return;

        attackSelection.SetActive(false);
        attackInfoPanel.SetActive(false);
        attackInfo.text = "";
        Battle.nextUIType = Battle.UI_skillUsed;
        Battle.usedPlayerSkill = Battle.player.skill[n];
        Battle.waitingEnded = true;
    }

    public void SelectItem()
    {
        itemSelection.SetActive(true);
    }

    public void HideSelectPanel()
    {
        itemSelection.SetActive(false);
        attackSelection.SetActive(false);
        attackInfoPanel.SetActive(false);
    }

    public void Item(int n)
    {
        if (n != 1 && n != 0) return;
        if (n == 0 && Battle.heal.skillPP <= 0) return;
        if (n == 1 && Battle.heal2.skillPP <= 0) return;

        Battle.nextUIType = Battle.UI_skillUsed;
        if (n == 0) Battle.usedPlayerSkill = Battle.heal;
        if (n == 1) Battle.usedPlayerSkill = Battle.heal2;
        Battle.waitingEnded = true;
        itemSelection.SetActive(false);
    }

    public void EndWaiting()
    {
        Battle.waitingEnded = true;
    }
    public void RunAway()
    {
        Battle.nextUIType = Battle.UI_run;
        Battle.waitingEnded = true;
    }
}
