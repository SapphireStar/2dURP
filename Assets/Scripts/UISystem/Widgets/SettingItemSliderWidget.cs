using Isekai.Datas;
using Isekai.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Isekai.UI.Views.Widgets
{
    public class SettingItemSliderWidget : MonoBehaviour
    {
        private Action<float> m_onValueChanged;
        [SerializeField]
        private Slider m_slider;
        [SerializeField]
        private TextMeshProUGUI m_title;

        private string m_settingKey;
        public void Initialize(UserSettingItem item)
        {
            m_settingKey = item.SettingName;
            m_slider.minValue = item.MinValue;
            m_slider.maxValue = item.MaxValue;
            m_slider.value = SettingsManager.Instance.GetSettingByKey<float>(m_settingKey);
            m_title.text = item.Title;
        }
        void Update()
        {

        }
        public void OnValueChanged()
        {
            SettingsManager.Instance.SetSettingByKey<float>(m_settingKey, m_slider.value);
            m_onValueChanged?.Invoke(m_slider.value);
        }

        public void Subscribe(Action<float> callback)
        {
            if (m_onValueChanged == null)
            {
                m_onValueChanged = callback;
            }
            else
            {
                m_onValueChanged += callback;
            }
        }
        public void Unsubscribe(Action<float> callback)
        {
            if (m_onValueChanged == null)
            {
                return;
            }

                m_onValueChanged -= callback;
            
        }
    }

}
