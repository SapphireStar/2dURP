using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : BaseStateMachine<PlayerStates>
{
    public PlayerStateMachine(MonoBehaviour owner)
    {
        m_owner = owner;
    }
    public PlayerStateMachine(MonoBehaviour owner, BaseState<PlayerStates> initialState)
    {
        m_owner = owner;
        m_curState = initialState;
    }

}
