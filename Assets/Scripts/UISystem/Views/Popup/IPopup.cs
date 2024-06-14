using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPopup
{
    PopupData Data { get; set; }
    void OnConfirmClicked();
    void OnCancelClicked();
}
