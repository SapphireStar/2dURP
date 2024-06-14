using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isekai.Datas
{
    [Serializable]
    public class UserSettings
    {
        public UserSetting<bool> MusicOn = new UserSetting<bool>(true);
        public UserSetting<float> MusicVolume = new UserSetting<float>(20);
        public UserSetting<bool> SoundOn = new UserSetting<bool>(true);
        public UserSetting<float> SoundVolume = new UserSetting<float>(20);
    }
    public class SettingChangedEventArg<T>
    {
        public SettingChangedEventArg(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue;
        public T NewValue;

    }
    [Serializable]
    public class UserSetting<T>
    {
        public UserSetting(T initialValue)
        {
            Value = initialValue;
        }
        //If want to convert the variable in class to JSON field, then must add SerializeField Attribute
        [SerializeField]
        private T m_value;
        public T Value
        {
            get => m_value;
            set
            {
                SettingChangedEvent?.Invoke(new SettingChangedEventArg<T>(m_value,value));
                m_value = value;
            }
        }

        public delegate void SettingEventHandler(SettingChangedEventArg<T> arg);
        public SettingEventHandler SettingChangedEvent;
        public void Subscribe(SettingEventHandler callback)
        {
            if (SettingChangedEvent == null)
            {
                SettingChangedEvent = callback;
                return;
            }
            SettingChangedEvent += callback;
        }

        public void Unsubscribe(SettingEventHandler callback)
        {
            if (SettingChangedEvent == null)
            {
                return;
            }
            SettingChangedEvent -= callback;
        }
    }
}

