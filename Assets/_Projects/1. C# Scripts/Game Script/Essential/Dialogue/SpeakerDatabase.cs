using UnityEngine;
using System;

public enum SpeakerType
{
    Giovanna,
    Marco,
    System,
    MC,
    MafiaBoss,
    Customer
}

[CreateAssetMenu(fileName = "SpeakerDatabase", menuName = "Dialogue/Speaker Database")]
public class SpeakerDatabase : ScriptableObject
{
    [System.Serializable]
    public class SpeakerEntry
    {
        public string displayName;
        public SpeakerType type;
        public Texture2D icon;
        public Color textColor = Color.white;
    }

    [SerializeField] private SpeakerEntry[] speakers;

    public SpeakerEntry GetSpeaker(SpeakerType type){
        foreach(var speaker in speakers){
            if(speaker.type == type) return speaker;
        }
        return null;
    }
}