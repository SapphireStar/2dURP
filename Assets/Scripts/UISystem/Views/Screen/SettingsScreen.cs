using Cysharp.Threading.Tasks;
using Isekai.Datas;
using Isekai.Managers;
using Isekai.UI.ViewModels.Screens;
using Isekai.UI.Views.Widgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.UI.Views.Screens
{
    public class SettingsScreen : Screen<SettingsViewModel>
    {
        [SerializeField]
        private Transform m_settingsContainer;

        [SerializeField]
        private GameObject m_settingItemToggleWidget;
        [SerializeField]
        private GameObject m_settingItemSliderWidget;

        UserSettingsData m_data;
        public void OnBackClicked()
        {
            ViewModel.SaveSettings();
            ScreenManager.Instance.BackToPrevScreen();
        }
        public override void OnEnterScreen()
        {
            
        }

        protected override void RefreshAll()
        {
            m_data = ViewModel.Data;
            for (int i = 0; i < m_data.UserSettingItems.Length; i++)
            {
                UserSettingItem item = m_data.UserSettingItems[i];
                GameObject prefab = GetSettingPrefab(item.Type);
                GameObject go = Instantiate(prefab, m_settingsContainer);
                InitializeSettingObject(go, item);
                for (int j = 0; j < item.SubSettings.Length; j++)
                {
                    UserSettingItem subitem = item.SubSettings[j];
                    GameObject subprefab = GetSettingPrefab(subitem.Type);
                    GameObject subgo = Instantiate(subprefab, m_settingsContainer);
                    InitializeSettingObject(subgo, subitem);
                }
            }
        }
        GameObject GetSettingPrefab(EValueType type)
        {
            switch (type)
            {
                case EValueType.Boolean:
                    return m_settingItemToggleWidget;
                case EValueType.Float:
                    return m_settingItemSliderWidget;
                case EValueType.Custom:
                    break;
                default:
                    break;
            }
            Debug.LogErrorFormat("Can't find prefab for SettingsValueType: {0}", type.ToString());
            return null;
        }

        void InitializeSettingObject(GameObject go,  UserSettingItem item)
        {
            switch (item.Type)
            {
                case EValueType.Boolean:
                    SettingItemToggleWidget toggleWidget = go.GetComponent<SettingItemToggleWidget>();
                    toggleWidget.Initialize(item);
                    break;
                case EValueType.Float:
                    SettingItemSliderWidget sliderWidget = go.GetComponent<SettingItemSliderWidget>();
                    sliderWidget.Initialize(item);
                    if (item.SettingName == "SoundVolume")
                    {
                        sliderWidget.Subscribe(OnSoundValueChanged);
                    }
                    break;
                default:
                    break;
            }
        }

        float m_soundVolumeNotifyCD = 0.1f;
        float m_curtime = 0;
        public void OnSoundValueChanged(float arg)
        {
            if (m_curtime <= 0)
            {
                m_curtime = m_soundVolumeNotifyCD;
                SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_icon_hover);
                StartCoolingDown().Forget();
            }
            
        }

        async UniTaskVoid StartCoolingDown()
        {
            while (m_curtime > 0)
            {
                m_curtime -= Time.deltaTime;
                await UniTask.Yield(this.GetCancellationTokenOnDestroy());
            }
        }
        public override void OnExitScreen()
        {
            
        }

    }

}
