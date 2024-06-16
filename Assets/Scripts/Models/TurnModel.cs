using System.Collections;
using System.Collections.Generic;
using Isekai.UI.Models;
using UnityEngine;

public class TurnModel : Model
{
    public TurnModel()
    {

    }
    private int m_order = 0;
    public int Order
    {
        get => m_order;
        set
        {
            ChangePropertyAndNotify<int>(ref m_order, value);
        }
    }
}
