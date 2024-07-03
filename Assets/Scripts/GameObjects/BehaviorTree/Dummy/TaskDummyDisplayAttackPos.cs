using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskDummyDisplayAttackPos : Action
{
    private AttackComponent attackComponent;
    public override void OnAwake()
    {
        attackComponent = GetComponent<AttackComponent>();
    }
    public override void OnStart()
    {
        if(attackComponent == null)
        {
            Debug.LogError("Character has no AttackComponent");
            return;
        }
        attackComponent.DisplayAttackPos(TurnManager.Instance.Player.CurPos);
    }
}
