using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskDummyEndTurn : Action
{
    private BaseTurnBasedCharacter character;
    public SharedBool IsInTurn;
    public override void OnAwake()
    {


    }
    public override TaskStatus OnUpdate()
    {
        if (IsInTurn.Value)
        {
            character = GetComponent<BaseTurnBasedCharacter>();

            character.EndTurn();
        }
        if (!IsInTurn.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
