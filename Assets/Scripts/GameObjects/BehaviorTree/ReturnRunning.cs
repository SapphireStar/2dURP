using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ReturnRunning : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
