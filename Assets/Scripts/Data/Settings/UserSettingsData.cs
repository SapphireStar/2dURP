using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Datas
{
    public enum EValueType
    {
        Boolean,
        Float,
        Custom,
    }
    [Serializable]
    public class UserSettingItem
    {
        public string Title;
        public string SettingName;
        public EValueType Type;
        public float MinValue;
        public float MaxValue;
        public float IncrementSize;
        public GameObject CustomSettingItemPrefab;
        public UserSettingItem[] SubSettings;
    }
    [CreateAssetMenu(fileName = "UserSettingsData", menuName = "Data/Settings/UserSettingsData", order = 4)]
    public class UserSettingsData : ScriptableObject
    {
        public UserSettingItem[] UserSettingItems;
    }
}
