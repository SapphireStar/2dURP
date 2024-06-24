using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T>
{
    public BaseStateMachine<T> m_stateMachine;
    public BaseState()
    {

    }
    public BaseState(BaseStateMachine<T> stateMachine)
    {
        m_stateMachine = stateMachine;
    }
    public abstract void EnterState();
    public abstract void OnUpdate();
    public abstract void ExitState();

}
