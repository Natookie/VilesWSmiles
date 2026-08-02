using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [Header("DATABASE")]
    [SerializeField] private LocalizationDatabase database;

    public System.Action OnLanguageChanged;

    private Dictionary<string, LocalizationEntry> entryMap;
    private int currentLanguageIndex;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetLanguage(SaveManager.Instance.CurrentData.languageIndex);
        BuildEntryMap();
    }

    void Start(){
        currentLanguageIndex = SaveManager.Instance.CurrentData.languageIndex;
        currentLanguageIndex = Mathf.Clamp(currentLanguageIndex, 0, database.supportedLanguages.Length - 1);
    }

    void BuildEntryMap(){
        entryMap = new Dictionary<string, LocalizationEntry>();
        foreach(var entry in database.entries){
            if(!entryMap.ContainsKey(entry.key)) entryMap[entry.key] = entry;
        }
    }

    public string GetText(string key){
        if(entryMap.TryGetValue(key, out LocalizationEntry entry)){
            if(currentLanguageIndex < entry.translations.Length) return entry.translations[currentLanguageIndex];
            return entry.translations[0];
        }
        return key;
    }

    public void SetLanguage(int index){
        if(index < 0 || index >= database.supportedLanguages.Length) return;

        currentLanguageIndex = index;
        SaveManager.Instance.CurrentData.languageIndex = index;
        SaveManager.Instance.Save();
        OnLanguageChanged?.Invoke();
    }

    public int GetCurrentLanguageIndex() => currentLanguageIndex;
    public SystemLanguage[] GetSupportedLanguages() => database.supportedLanguages;
}