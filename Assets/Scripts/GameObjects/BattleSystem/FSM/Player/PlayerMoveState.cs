using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : BaseState<PlayerStates>
{
    float timerInterval;
    float timer;

    PlayerTurnBasedController owner;
    public PlayerMoveState(float timerInterval)
    {
        this.timerInterval = timerInterval;
    }
    public override void EnterState()
    {
        owner = m_stateMachine.Owner.GetComponent<PlayerTurnBasedController>();
        if (owner == null)
        {
            Debug.LogError("Player doesn't has PlayerTurnBasedController");
        }
    }

    public override void ExitState()
    {
         owner.ClearMouseHoverRoute();
    }

    public override void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timerInterval;
            MouseHoverRoute();
        }
        ClickMove();
    }



}
