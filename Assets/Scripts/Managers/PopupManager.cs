using Cysharp.Threading.Tasks;
using Isekai.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    PausePopup,
    StartDeliverPopup,
    SettingsPopup,
    WinPopup
}
public class PopupData
{
    public Action OnCancelClicked;
    public Action OnConfirmClicked;
}
public class PopupManager : MonoSingleton<PopupManager>
{
    public async UniTask<T> ShowPopup<T>(PopupType Type, PopupData data) where T:MonoBehaviour,IPopup
    {
        var prefab = await ResourceManager.Instance.LoadResourceAsync<GameObject>(Type.ToString()) as GameObject;
        if(prefab == null)
        {
            Debug.Log("Can't find the Popup");
            return null;
        }
        T popup = Instantiate(prefab, LayerManager.Instance.GetLayer(ELayerType.PopupLayer)).GetComponent<T>();
        popup.Data = data;
        return popup;
    }
}
