using UnityEngine;

[System.Serializable]
public class LocalizationEntry
{
    public string key;
    public string[] translations;
}

[CreateAssetMenu(fileName = "LocalizationDatabase", menuName = "Localization/Localization Database")]
public class LocalizationDatabase : ScriptableObject
{
    public SystemLanguage[] supportedLanguages;
    public LocalizationEntry[] entries;
}