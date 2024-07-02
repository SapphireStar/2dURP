using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(ObjectStatsComponent))]
public class ConditionIsInTurn : Action
{
    public SharedBool IsInTurn;
    public override void OnAwake()
    {

    }
    public override TaskStatus OnUpdate()
    {
        if (IsInTurn.Value)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Running;
        }
    }
}
