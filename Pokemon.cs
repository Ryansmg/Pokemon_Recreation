using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public string name;
    public int type;
    public int hp;
    public int maxHp;
    public int speed;
    /// <summary>
    /// Hp가 이 프레임에 변경됐는지를 나타냄.
    /// </summary>
    public bool hpChanged;

    public Skill[] skill;

    public Pokemon(string name_, int type_, int maxHp_, int speed_, Skill sk1, Skill sk2, Skill sk3, Skill sk4)
    {
        name = name_;
        type = type_;
        hp = maxHp_;
        maxHp = maxHp_;
        speed = speed_;
        hpChanged = false;

        skill = new Skill[4];
        skill[0] = sk1;
        skill[1] = sk2;
        skill[2] = sk3;
        skill[3] = sk4;
    }
}
