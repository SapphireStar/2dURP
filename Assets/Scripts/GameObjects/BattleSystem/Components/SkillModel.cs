using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModel
{
    public float PhysicalDamage;
    public float MagicalDamage;

    public List<Point> Targets;

    public SkillModel()
    {
        Targets = new List<Point>();
    }
}
