using System;
using System.Collections;
using System.Collections.Generic;
using Isekai.UI.Models;
using UnityEngine;

public class ModelManager : MonoSingleton<ModelManager>
{
    private Dictionary<Type, Model> m_models;
    protected override void OnStart()
    {
        base.OnStart();
        m_models = new Dictionary<Type, Model>();
        RegisterModel(typeof(TurnModel), new TurnModel());
        RegisterModel(typeof(PlayerStatsModel), new PlayerStatsModel());
    }

    public T GetModel<T>(Type modelType) where T:Model
    {
        Model model;
        m_models.TryGetValue(modelType, out model);
        if(model != null)
        {
            return (T)model;
        }
        else
        {
            Debug.Log($"Model {nameof(T)} is not registered");
            return null;
        }
    }
    public void RegisterModel(Type modelType, Model model)
    {
        if (m_models.ContainsKey(modelType))
        {
            m_models[modelType] = model;
        }
        else
        {
            m_models[modelType] = model;
        }
    }
}
