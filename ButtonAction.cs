using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public GameObject attackSelection;
    public void AttackTest()
    {
        Battle.nextUIType = Battle.UI_skillUsed;
        Battle.usedPlayerSkill = Battle.player.skill[0];
        Battle.waitingEnded = true;
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
        }
    }

    public void Attack(int n)
    {
        attackSelection.SetActive(false);
        Battle.nextUIType = Battle.UI_skillUsed;
        Battle.usedPlayerSkill = Battle.player.skill[n];
        Battle.waitingEnded = true;
    }

    public void Item()
    {
        Battle.nextUIType = Battle.UI_skillUsed;
        Battle.usedPlayerSkill = Battle.heal;
        Battle.waitingEnded = true;
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
