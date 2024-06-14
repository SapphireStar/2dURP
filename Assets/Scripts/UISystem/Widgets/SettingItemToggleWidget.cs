using Isekai.Datas;
using Isekai.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Isekai.UI.Views.Widgets
{
    public class SettingItemToggleWidget : MonoBehaviour
    {
        private Action<bool> m_onValueChanged;
        [SerializeField]
        private TextMeshProUGUI m_title;
        [SerializeField]
        private Toggle m_toggle;

        private string m_settingKey;
        public void Initialize(UserSettingItem item)
        {
            m_settingKey = item.SettingName;
            m_title.text = item.Title;
            m_toggle.isOn = SettingsManager.Instance.GetSettingByKey<bool>(item.SettingName);
        }

        public void OnValueChanged()
        {
            SettingsManager.Instance.SetSettingByKey<bool>(m_settingKey, m_toggle.isOn);
            m_onValueChanged?.Invoke(m_toggle.isOn);
        }
    }

}
