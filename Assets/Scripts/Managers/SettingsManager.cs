using Cysharp.Threading.Tasks;
using Isekai.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Isekai.Managers
{
    public class SettingsManager : Singleton<SettingsManager>
    {
        public UserSettingsData SettingData { get; private set; }
        private UserSettings m_curUserSettings;
        public UserSettings CurUserSettings => m_curUserSettings;
        private FieldInfo[] m_settingFieldInfos;
        private string m_savedSettingsKey = "UserSettings";
        public async UniTask Initialize()
        {
            SettingData = await ResourceManager.Instance.LoadResourceAsync<UserSettingsData>("UserSettingsData");
            LoadSettings();
            m_settingFieldInfos = m_curUserSettings.GetType().GetFields();  
        }

        public void SaveSettings()
        {
            string serializedSetting = JsonUtility.ToJson(m_curUserSettings);
            PlayerPrefs.SetString(m_savedSettingsKey, serializedSetting);
        }
        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey(m_savedSettingsKey))
            {
                m_curUserSettings = JsonUtility.FromJson<UserSettings>(PlayerPrefs.GetString(m_savedSettingsKey));
                return;
            }
            m_curUserSettings = new UserSettings();
        }
        public T GetSettingByKey<T>(string key)
        {
            for (int i = 0; i < m_settingFieldInfos.Length; i++)
            {
                if(m_settingFieldInfos[i].Name == key)
                {
                    UserSetting<T> setting = (UserSetting<T>)m_settingFieldInfos[i].GetValue(m_curUserSettings);
                    return setting.Value;
                }
            }
            Debug.LogErrorFormat("Can't find the key {0} in UserSettings. Please check the key is valid or not.", key);
            return default(T);
        }

        public void SetSettingByKey<T>(string key, T value)
        {
            for (int i = 0; i < m_settingFieldInfos.Length; i++)
            {
                if (m_settingFieldInfos[i].Name == key)
                {
                    UserSetting<T> setting = (UserSetting<T>)m_settingFieldInfos[i].GetValue(m_curUserSettings);
                    setting.Value = value;
                    return;
                }
            }
            Debug.LogErrorFormat("Can't find the key {0} in UserSettings. Please check the key is valid or not.", key);
        }
    }

}
