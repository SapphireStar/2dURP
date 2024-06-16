using System.Collections;
using System.Collections.Generic;
using Isekai.UI.Models;
using UnityEngine;

public class PlayerStatsModel : Model
{
    private int m_maxSteps;
    private int m_remainSteps;
    private bool m_isMoving;

    public PlayerStatsModel()
    {
        m_maxSteps = 10;
    }
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
}
