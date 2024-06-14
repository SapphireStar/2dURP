using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Isekai.UI.ExtendedWidgets
{
    public class ExtendedButton : Button
    {
        public UnityEvent OnClick;
        public UnityEvent OnDoubleClick;
        public float DoubleClickGap = 0.2f;

        protected override void Start()
        {
            base.Start();
            CheckDoubleClick(this).Forget();
        }
        [SerializeField]
        private TextMeshProUGUI m_btnText;

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_icon_hover);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Confirm);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public void SetButtonText(string text)
        {
            m_btnText.text = text;
        }

        async UniTaskVoid CheckDoubleClick(Button button)
        {
            while (true)
            {
                var firstClickAsync = button.OnClickAsync();
                await firstClickAsync;
                Debug.Log("按钮第一次被点击");
                var secondClickAsync = button.OnClickAsync();
                int resultIndex = await UniTask.WhenAny(secondClickAsync, UniTask.Delay(TimeSpan.FromSeconds(DoubleClickGap),false,PlayerLoopTiming.Update,this.GetCancellationTokenOnDestroy()));
                if (resultIndex == 0)
                {
                    OnDoubleClick?.Invoke();
                    Debug.Log("触发双击");
                }
                else
                {
                    OnClick?.Invoke();
                    Debug.Log("超时");
                }
            }
        }
        public void onClickself()
        {
            Debug.Log("clicked");
        }
    }
}


