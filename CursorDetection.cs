using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetection : MonoBehaviour
{
    public int n;
    public void CursorEnter()
    {
        ButtonAction.attackInfo.text = n switch
        {
            0 => "스킬 특성:\n자속 보정으로 위력 150%\n\n명중률: 100%",
            1 => "스킬 특성:\n반드시 선제공격할 수 있다\n\n명중률: 100%",
            2 => "스킬 특성:\n자속 보정으로 위력 150%\n\n명중률: 80%",
            3 => "스킬 특성:\n자속 보정으로 위력 150%\n급소에 맞을 확률이 높다\n명중률: 100%",
            _ => ""
        };
    }
    public void CursorExit()
    {
        ButtonAction.attackInfo.text = "";
    }
}
