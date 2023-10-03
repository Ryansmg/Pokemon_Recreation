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
            0 => "��ų Ư��:\n�ڼ� �������� ���� 150%\n\n���߷�: 100%",
            1 => "��ų Ư��:\n�ڼ� �������� ���� 150%\n\n���߷�: 70%",
            2 => "��ų Ư��:\n����\n\n���߷�: 75%",
            3 => "��ų Ư��:\n�ݵ�� ���������� �� �ִ�\n\n���߷�: 100%",
            _ => ""
        };
    }
    public void CursorExit()
    {
        ButtonAction.attackInfo.text = "";
    }
}