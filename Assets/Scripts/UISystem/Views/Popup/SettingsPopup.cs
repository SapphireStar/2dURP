using Isekai.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour, IPopup
{
    [SerializeField]
    private GameObject m_muteMusic;
    [SerializeField]
    private GameObject m_muteSound;
    public PopupData Data { get; set; }
    private void Start()
    {
        m_muteMusic.SetActive(!SettingsManager.Instance.GetSettingByKey<bool>("MusicOn"));
        m_muteSound.SetActive(!SettingsManager.Instance.GetSettingByKey<bool>("SoundOn"));
    }

    public void OnMusicClicked()
    {
        bool musicIsOn = SettingsManager.Instance.GetSettingByKey<bool>("MusicOn");
        SettingsManager.Instance.SetSettingByKey<bool>("MusicOn", !musicIsOn);
        SoundManager.Instance.MusicOn = !musicIsOn;

        m_muteMusic.SetActive(musicIsOn);
    }

    public void OnSoundClicked()
    {
        bool soundIsOn = SettingsManager.Instance.GetSettingByKey<bool>("SoundOn");
        SettingsManager.Instance.SetSettingByKey<bool>("SoundOn", !soundIsOn);
        SoundManager.Instance.SoundOn = !soundIsOn;

        m_muteSound.SetActive(soundIsOn);
    }

    public void OnCloseClicked()
    {
        SettingsManager.Instance.SaveSettings();
        Destroy(gameObject);
    }

    public void OnCancelClicked()
    {
        Data.OnCancelClicked?.Invoke();
        SettingsManager.Instance.SaveSettings();
    }

    public void OnConfirmClicked()
    {
        Data.OnConfirmClicked?.Invoke();
        SettingsManager.Instance.SaveSettings();
    }
}
