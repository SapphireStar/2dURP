using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskDummySetSkill : Action
{
    public SkillData CurSkillData;
    private AttackComponent attackComponent;
    public override void OnAwake()
    {
        attackComponent = GetComponent<AttackComponent>();
    }
    public override void OnStart()
    {
        attackComponent.SetSkill(CurSkillData);
    }
}
