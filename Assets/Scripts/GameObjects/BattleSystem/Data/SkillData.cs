using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSkillData",menuName = "Data/GameObject/NewSkillData")]
public class SkillData : ScriptableObject
{
    public float PhysicalDamage;
    public float MagicalDamage;

    public int RangeRadius;
    public bool MainAction;
    public bool BonusAction;

    public string AnimTrigger;

    public bool CanApplySelf;
}

