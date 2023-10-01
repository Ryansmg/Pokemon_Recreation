using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void AttackTest()
    {
        Battle.nextUIType = Battle.UI_skillUsed;
        Battle.usedPlayerSkill = Battle.player.skill[0];
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
