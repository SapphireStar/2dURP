using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    [SerializeField]
    private PlayerInput m_playerInput;
    public void Awake()
    {
        //m_playerInput = new PlayerInput();
    }
    public PlayerInput PlayerInput
    {
        get
        {
            if (m_playerInput != null)
            {
                return m_playerInput;
            }
            Debug.Log("PlayerInput not initialized.");
            return null;
        }
    }
}
