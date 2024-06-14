using Isekai.Datas;
using Isekai.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioSource soundAudioSource;

    const string MusicPath = "Music/";
    const string SoundPath = "Sound/";

    private  bool musicOn;
    public  bool MusicOn
    {
        get
        {
            return musicOn;
        }
        set
        {
            musicOn = value;
            this.MusicMute(!musicOn);
        }
    }

    private bool soundOn;
    public bool SoundOn
    {
        get
        {
            return soundOn;
        }
        set
        {
            soundOn = value;
            this.SoundMute(!soundOn);
        }
    }
    private int musicVolume;
    public int MusicVolume
    {
        get
        {
            return MusicVolume;
        }
        set
        {
            musicVolume = value;
            if (musicOn)
            {
                this.SetVolume("MusicVolume", musicVolume);
            }
        }
    }

    private int soundVolume;
    public int SoundVolume
    {
        get
        {
            return SoundVolume;
        }
        set
        {
            soundVolume = value;
            if (soundOn)
            {
                this.SetVolume("SoundVolume", soundVolume);
            }
        }
    }
    private void Start()
    {

    }

    public void Initialize()
    {
        SettingsManager.Instance.CurUserSettings.MusicOn.Subscribe(HandleMusicOn);
        SettingsManager.Instance.CurUserSettings.MusicVolume.Subscribe(HandleMusicVolume);
        SettingsManager.Instance.CurUserSettings.SoundOn.Subscribe(HandleSoundOn);
        SettingsManager.Instance.CurUserSettings.SoundVolume.Subscribe(HandleSoundVolume);
        MusicOn = SettingsManager.Instance.CurUserSettings.MusicOn.Value;
        MusicVolume = (int)SettingsManager.Instance.CurUserSettings.MusicVolume.Value;
        SoundOn = SettingsManager.Instance.CurUserSettings.SoundOn.Value;
        SoundVolume = (int)SettingsManager.Instance.CurUserSettings.SoundVolume.Value;

    }

    void HandleMusicOn(SettingChangedEventArg<bool> arg)
    {  
        Debug.LogFormat("Set Music On:{0}", arg.NewValue.ToString());
        MusicOn = arg.NewValue;
    }
    void HandleMusicVolume(SettingChangedEventArg<float> arg)
    {
        Debug.LogFormat("Set Music Volume:{0}", arg.NewValue.ToString());
        MusicVolume = (int)arg.NewValue;
    }
    void HandleSoundOn(SettingChangedEventArg<bool> arg)
    {
        Debug.LogFormat("Set Sound On:{0}", arg.NewValue.ToString());
        SoundOn = arg.NewValue;
    }
    void HandleSoundVolume(SettingChangedEventArg<float> arg)
    {
        Debug.LogFormat("Set Sound Volume:{0}", arg.NewValue.ToString());
        SoundVolume = (int)arg.NewValue;
    }

    public void MusicMute(bool mute)
    {
        this.SetVolume("MusicVolume", mute ? 0 : musicVolume);
    }
    public void SoundMute(bool mute)
    {
        this.SetVolume("SoundVolume", mute ? 0 : soundVolume);
    }

    private void SetVolume(string name,int value)
    {
        if (value == 0)
        {
            this.audioMixer.SetFloat(name, -100);
            return;
        }
        var extractedvalue = value * 5;
        float volume = extractedvalue * 0.25f - 20f;
        this.audioMixer.SetFloat(name, volume);
    }

    public async void PlayMusic(string name)
    {
        AudioClip clip = await ResourceManager.Instance.LoadResourceAsync<AudioClip>(name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlayMusic:{0} not existed", name);
            return;
        }
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public async void PlaySound(string name)
    {
        AudioClip clip = await ResourceManager.Instance.LoadResourceAsync<AudioClip>(name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlaySound:{0} not existed", name);
            return;
        }
        soundAudioSource.PlayOneShot(clip);

    }
}
