using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskDummyUseSkill : Action
{
    private AttackComponent attackComponent;
    public override void OnAwake()
    {
        attackComponent = GetComponent<AttackComponent>();
    }
    public override TaskStatus OnUpdate()
    {
        if (TurnManager.Instance.Player != null)
        {
            attackComponent.SetTarget(TurnManager.Instance.Player.CurPoint);
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
