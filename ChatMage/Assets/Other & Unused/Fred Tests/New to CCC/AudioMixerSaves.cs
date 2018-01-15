using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using CCC.Persistence;

[CreateAssetMenu(menuName = "Audio/Audio Mixer Saves")]
public class AudioMixerSaves : ScriptablePersistent
{
    [System.Serializable]
    public struct Settings
    {
        public Channel master;
        public Channel voice;
        public Channel sfx;
        public Channel music;
        public Settings(Channel master, Channel voice, Channel sfx, Channel music)
        {
            this.master = master;
            this.voice = voice;
            this.sfx = sfx;
            this.music = music;
        }

        public static Settings DefaultSettings()
        {
            return new Settings()
            {
                master = new Channel(0, false),
                voice = new Channel(0, false),
                sfx = new Channel(0, false),
                music = new Channel(0, false)
            };
        }
    }
    [System.Serializable]
    public struct Channel
    {
        public float dbBoost;
        public bool muted;
        public Channel(float dbBoost, bool muted)
        {
            this.dbBoost = dbBoost;
            this.muted = muted;
        }
    }

    [SerializeField] private string fileName = "Audio Settings";
    [SerializeField] private bool loadOnInit = true;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Settings defaultSettings;
    [NonSerialized] private Settings settings;

    public Settings GetCurrentSettings() { return settings; }
    public Settings SetNewSettings(Settings newSettings) { return settings = newSettings; }

    public override void Init(Action onComplete)
    {
        if (loadOnInit)
            Load();
        onComplete();
    }

    #region Set
    public void SetMaster(float dbBoost)
    {
        settings.master.dbBoost = dbBoost;
        ApplyMaster();
    }
    public void SetMaster(bool muted)
    {
        settings.master.muted = muted;
        ApplyMaster();
    }
    public void SetSFX(float dbBoost)
    {
        settings.sfx.dbBoost = dbBoost;
        ApplySFX();
    }
    public void SetSFX(bool muted)
    {
        settings.sfx.muted = muted;
        ApplySFX();
    }
    public void SetVoice(float dbBoost)
    {
        settings.voice.dbBoost = dbBoost;
        ApplyVoice();
    }
    public void SetVoice(bool muted)
    {
        settings.voice.muted = muted;
        ApplyVoice();
    }
    public void SetMusic(float dbBoost)
    {
        settings.music.dbBoost = dbBoost;
        ApplyMusic();
    }
    public void SetMusic(bool muted)
    {
        settings.music.muted = muted;
        ApplyMusic();
    }
    #endregion

    #region Apply
    private const int MUTE_VOLUME = -80;
    public void ApplyAll()
    {
        ApplyMaster();
        ApplyMusic();
        ApplyVoice();
        ApplySFX();
    }
    public void ApplyMaster()
    {
        float val = MasterMuted ? MUTE_VOLUME : settings.master.dbBoost;
        mixer.SetFloat("master", val);
    }
    public void ApplySFX()
    {
        float val = SFXMuted ? MUTE_VOLUME : settings.sfx.dbBoost;
        mixer.SetFloat("sfx", val);
        mixer.SetFloat("static sfx", val);
    }
    public void ApplyVoice()
    {
        float val = VoiceMuted ? MUTE_VOLUME : settings.voice.dbBoost;
        mixer.SetFloat("voice", val);
    }
    public void ApplyMusic()
    {
        float val = MusicMuted ? MUTE_VOLUME : settings.music.dbBoost;
        mixer.SetFloat("music", val);
    }

    private bool MasterMuted { get { return settings.master.muted; } }
    private bool MusicMuted { get { return settings.music.muted || MasterMuted; } }
    private bool VoiceMuted { get { return settings.voice.muted || MasterMuted; } }
    private bool SFXMuted { get { return settings.sfx.muted || MasterMuted; } }
    #endregion

    #region Save / Load
    private string FileName
    {
        get { return "/" + fileName + ".dat"; }
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + FileName;
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            Settings saveCopy = (Settings)bf.Deserialize(file);

            settings = saveCopy;

            file.Close();

            ApplyAll();
        }
        else
        {
            SetDefaults();
            Save();
        }
    }

    public bool Save()
    {
        try
        {
            string savePath = Application.persistentDataPath + FileName;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.OpenOrCreate);
            bf.Serialize(file, settings);
            file.Close();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void SetDefaults()
    {
        settings = defaultSettings;
    }
    #endregion
}
