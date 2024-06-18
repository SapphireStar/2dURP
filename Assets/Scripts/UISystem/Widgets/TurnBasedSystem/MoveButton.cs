using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image ImageBackground;
    private bool mouseIn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIn = true;
        ImageBackground.color = new Color(1, 1, 1, 0.4f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIn = false;
        ImageBackground.color = new Color(1, 1, 1, 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ImageBackground.color = new Color(1, 1, 1, 0.7f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!mouseIn)
        {
            return;
        }
        ImageBackground.color = new Color(1, 1, 1, 0.4f);
        TurnManager.Instance.Player.OnClickMoveButton();
    }




}
