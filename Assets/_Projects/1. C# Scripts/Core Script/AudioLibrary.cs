using UnityEngine;

public enum BGMTrack
{
    MainMenu,
    Gameplay
}

public enum SFXSound
{
    DoorChime,
    AddIce,
    Cut,
    Garnish,
    Juice,
    PouringLiquid,
    Shake,
    Stir
}

public enum UISound
{
    ButtonClick,
    ButtonHover,
    PanelOpen,
    SliderDrag,
    ObjectFailed,
    ObjectHovered,
    CustomerOrdered
}

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public class BGMTrackEntry
    {
        public BGMTrack track;
        public AudioClip clip;
    }

    [System.Serializable]
    public class SFXSoundEntry
    {
        public SFXSound sound;
        public AudioClip clip;
    }

    [System.Serializable]
    public class UISoundEntry
    {
        public UISound sound;
        public AudioClip clip;
    }

    [SerializeField] private BGMTrackEntry[] bgmTracks;
    [SerializeField] private SFXSoundEntry[] sfxSounds;
    [SerializeField] private UISoundEntry[] uiSounds;

    public AudioClip GetBGM(BGMTrack track){
        foreach(var entry in bgmTracks) if(entry.track == track) return entry.clip;
        return null;
    }

    public AudioClip GetSFX(SFXSound sound){
        foreach(var entry in sfxSounds) if(entry.sound == sound) return entry.clip;
        return null;
    }

    public AudioClip GetUI(UISound sound){
        foreach(var entry in uiSounds) if(entry.sound == sound) return entry.clip;
        return null;
    }
}