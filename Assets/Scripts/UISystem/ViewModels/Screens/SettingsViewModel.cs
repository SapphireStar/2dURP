using Isekai.Datas;
using Isekai.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.ViewModels.Screens
{
    public class SettingsViewModel : ViewModel
    {
        private UserSettingsData m_data;
        public UserSettingsData Data
        {
            get => m_data;
        }
        public SettingsViewModel()
        {
            m_data = SettingsManager.Instance.SettingData;
        }

        public void SaveSettings()
        {
            SettingsManager.Instance.SaveSettings();
        }



    }

}
