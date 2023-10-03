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
