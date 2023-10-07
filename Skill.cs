using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    static List<Skill> allSkills = new();

    public const int type_normal = -1;
    public const int type_fire = 0;
    public const int type_water = 1;
    public const int type_electric = 2;
    public const int type_iron = 3;
    public const int type_dark = 4;
    public const int type_grass = 5;
    public const int type_ice = 6;
    public const int type_dragon = 7;
    public const int type_ground = 8;
    public const int type_rock = 9;
    public const int type_bug = 10;
    public const int type_fairy = 11;

    public int skillPP;
    public int skillMaxPP;
    public int skillDamage;
    public int skillType;
    public string skillName;
    public int skillAccuracyPercent;

    public Skill(int pp, int damage, int type, int accuracy, string name)
    {
        skillPP = pp;
        skillDamage = damage;
        skillType = type;
        skillName = name;
        skillMaxPP = pp;
        skillAccuracyPercent = accuracy;
        allSkills.Add(this);
    }

    public static void ResetSkills()
    {
        foreach(Skill skill in allSkills)
        {
            skill.skillPP = skill.skillMaxPP;
        }
    }
}
