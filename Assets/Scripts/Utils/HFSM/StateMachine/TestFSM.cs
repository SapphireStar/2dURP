using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFSM : BaseStateMachine<int>
{
    // Start is called before the first frame update
    IdleState idleState;
    MoveState moveState;
    DiedState diedState;

    void Start()
    {
        idleState = new IdleState();
        moveState = new MoveState();
        diedState = new DiedState();
        TransitionToStateWhen(1, idleState, moveState);
        TransitionToStateWhen(2, moveState, diedState);
        EnterState(idleState);
        Owner.StartCoroutine(test());
    }

    // Update is called once per frame
    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        m_curCondition = 1;
        Debug.Log(m_curCondition);
        Debug.Log(m_statesDict[m_curState][m_curCondition]);
        yield return new WaitForSeconds(1);
        m_curCondition = 2;
    }
}
public class IdleState : BaseState<int>
{
    public override void EnterState()
    {
        Debug.Log("Enter Idle State");
    }

    public override void ExitState()
    {
        Debug.Log("Exit Idle State");
    }

    public override void OnUpdate()
    {
    }
}
public class DiedState : BaseState<int>
{
    public override void EnterState()
    {
        Debug.Log("Enter DiedState");
    }

    public override void ExitState()
    {
        Debug.Log("Exit DiedState");
    }

    public override void OnUpdate()
    {
    }
}
public class MoveState : BaseState<int>
{
    public override void EnterState()
    {
        Debug.Log("Enter MoveState");
    }

    public override void ExitState()
    {
        Debug.Log("Exit MoveState");
    }

    public override void OnUpdate()
    {
    }
}