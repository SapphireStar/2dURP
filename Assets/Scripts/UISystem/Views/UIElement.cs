using Isekai.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIElement : MonoBehaviour
{

    public virtual void Initialize()
    {

    }

    public virtual ELayerType GetLayer()
    {
        return ELayerType.None;
    }
}
