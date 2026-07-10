using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("AUDIO MIXER")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("AUDIO LIBRARY")]
    [SerializeField] private AudioLibrary audioLibrary;

    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    [Header("DEFAULT VOLUMES")]
    [SerializeField, Range(0f, 1f)] private float defaultMasterVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float defaultBGMVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] private float defaultSFXVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float defaultUIVolume = 1f;

    private const string MasterParam = "MasterVolume";
    private const string BGMVolParam = "BGMVolume";
    private const string SFXVolParam = "SFXVolume";
    private const string UIVolParam = "UIVolume";

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateAudioSources();
    }

    void Start(){
        PlayBGM(BGMTrack.MainMenu);
    }

    void CreateAudioSources(){
        if(bgmSource == null){
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        if(sfxSource == null){
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if(uiSource == null){
            uiSource = gameObject.AddComponent<AudioSource>();
            uiSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("UI")[0];
            uiSource.loop = false;
            uiSource.playOnAwake = false;
        }
    }

    public void PlayBGM(BGMTrack track){
        AudioClip clip = audioLibrary.GetBGM(track);
        if(clip == null) return;

        if(bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(SFXSound sound){
        AudioClip clip = audioLibrary.GetSFX(sound);
        if(clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(SFXSound sound, float volumeScale){
        AudioClip clip = audioLibrary.GetSFX(sound);
        if(clip != null) sfxSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayUI(UISound sound){
        AudioClip clip = audioLibrary.GetUI(sound);
        if(clip != null) uiSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float volume){
        SetVolume(MasterParam, volume);
        SaveManager.Instance.CurrentData.masterVolume = volume;
        SaveManager.Instance.Save();
    }

    public void SetBGMVolume(float volume){
        SetVolume(BGMVolParam, volume);
        SaveManager.Instance.CurrentData.bgmVolume = volume;
        SaveManager.Instance.Save();
    }

    public void SetSFXVolume(float volume){
        SetVolume(SFXVolParam, volume);
        SaveManager.Instance.CurrentData.sfxVolume = volume;
        SaveManager.Instance.Save();
    }

    public void SetUIVolume(float volume){
        SetVolume(UIVolParam, volume);
        SaveManager.Instance.CurrentData.uiVolume = volume;
        SaveManager.Instance.Save();
    }

    void SetVolume(string parameter, float volume){
        float dB = volume > 0f ? Mathf.Log10(volume) * 20f : -80f;
        audioMixer.SetFloat(parameter, dB);
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MasterParam, defaultMasterVolume);
    public float GetBGMVolume() => PlayerPrefs.GetFloat(BGMVolParam, defaultBGMVolume);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFXVolParam, defaultSFXVolume);
    public float GetUIVolume() => PlayerPrefs.GetFloat(UIVolParam, defaultUIVolume);
}