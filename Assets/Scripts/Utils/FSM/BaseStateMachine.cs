using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine<T>
{
    protected MonoBehaviour m_owner;
    public MonoBehaviour Owner
    {
        get => m_owner;
        
    }

    protected BaseState<T> m_curState;
    public float StateRefreshInterval = 0.01f;
    private float stateRefreshTimer;

    public T m_curCondition;
    protected Dictionary<BaseState<T>, Dictionary<T, BaseState<T>>> m_statesDict;

    /// <summary>
    /// parent must have a none parameter constructor
    /// </summary>
    public BaseStateMachine()
    {

    }
    public BaseStateMachine(MonoBehaviour owner)
    {
        m_owner = owner;
    }
    public BaseStateMachine(MonoBehaviour owner, BaseState<T> initialState)
    {
        m_owner = owner;
        m_curState = initialState;
    }
    protected virtual void OnInitialized()
    {
        if (m_curState != null)
            EnterState(m_curState);
    }
    protected virtual void OnAwake()
    {
        m_statesDict = new Dictionary<BaseState<T>, Dictionary<T, BaseState<T>>>();
    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnUpdate()
    {
        if (stateRefreshTimer <= 0)
        {
            stateRefreshTimer = StateRefreshInterval;
            RefreshState();
        }
        stateRefreshTimer -= Time.deltaTime;

        m_curState.OnUpdate();
    }
    protected virtual void OnDestroy()
    {

    }
    public void TransitionToStateWhen(T condition, BaseState<T> from, BaseState<T> to)
    {
        if(!m_statesDict.ContainsKey(from))
        {
            m_statesDict[from] = new Dictionary<T, BaseState<T>>();
        }
        m_statesDict[from][condition] = to;
    }
    public void EnterState(BaseState<T> state)
    {
        if (m_curState != null)
        {
            m_curState.ExitState();
        }

        m_curState = state;

        m_curState.EnterState();
    }
    public void RefreshState()
    {
        if (!m_statesDict.ContainsKey(m_curState))
        {
            return;
        }
        if (m_statesDict[m_curState] == null)
        {
            return;
        }
        if (!m_statesDict[m_curState].ContainsKey(m_curCondition))
        {
            return;
        }
        BaseState<T> nextState = m_statesDict[m_curState][m_curCondition];
        EnterState(nextState);
        
    }
}
