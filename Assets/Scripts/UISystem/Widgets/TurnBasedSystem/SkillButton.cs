using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerUpHandler,IPointerDownHandler
{
    public Image ImageBackground;
    public SkillData CurSkillData;
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
        TurnManager.Instance.Player.OnClickSkillButton(CurSkillData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
