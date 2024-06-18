using System.Collections;
using System.Collections.Generic;
using Isekai.UI.Models;
using UnityEngine;

public class PlayerStatsModel : Model
{
    private PlayerStates m_playerState;
    

    #region Movement
    private int m_maxSteps;
    private int m_remainSteps;
    private bool m_isMoving;
    #endregion

    private int m_maxMainAction;
    private int m_maxBonusAction;
    private int m_remainMainAction;
    private int m_remainBonusAction;

    public PlayerStatsModel()
    {
        m_maxSteps = 10;
    }

    public PlayerStates PlayerState
    {
        get => m_playerState;
        set
        {
            ChangePropertyAndNotify<PlayerStates>(ref m_playerState, value);
        }
    }


    #region Attack
    public int MaxMainAction
    {
        get => m_maxMainAction;
        set
        {
            ChangePropertyAndNotify<int>(ref m_maxMainAction, value);
        }
    }

    public int RemainMainAction
    {
        get => m_remainMainAction;
        set
        {
            ChangePropertyAndNotify<int>(ref m_remainMainAction, value);
        }
    }

    public int MaxBonusAction
    {
        get => m_maxBonusAction;
        set
        {
            ChangePropertyAndNotify<int>(ref m_maxBonusAction, value);
        }
    }
    public int RemainBonusAction
    {
        get => m_remainBonusAction;
        set
        {
            ChangePropertyAndNotify<int>(ref m_remainBonusAction, value);
        }
    }
    #endregion

    #region Movement
    public int MaxSteps
    {
        get => m_maxSteps;
        set
        {
            ChangePropertyAndNotify<int>(ref m_maxSteps, value);
        }
    }

    public int RemainSteps
    {
        get => m_remainSteps;
        set
        {
            ChangePropertyAndNotify<int>(ref m_remainSteps, value);
        }
    }

    public bool IsMoving
    {
        get => m_isMoving;
        set
        {
            ChangePropertyAndNotify<bool>(ref m_isMoving, value);
        }
    }
    #endregion
}
