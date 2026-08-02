using UnityEngine;
using System;
using System.IO;

[Serializable]
public class GameSaveData
{
    //Volume
    public float masterVolume = 1f;
    public float bgmVolume = 0.8f;
    public float sfxVolume = 1f;
    public float uiVolume = 1f;
    //Resolution
    public int resolutionIndex = 2;
    public int languageIndex = 2;
    public float brightness = 0.85f;
    public bool useFullScreen = true;
    //Misc
    public int accumulatedMoney = 0;
    public int shiftIndex = 0;
    //State
    public bool hasCompletedTutorial = false;
    public bool hasFinishedGame = false;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public GameSaveData CurrentData { get; private set; }
    private string savePath;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
    }

    void Start(){
        // Debug.Log($"Save path: {savePath}");
        // Debug.Log($"Save exists: {File.Exists(savePath)}");
    }

    public void Save(){
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(savePath, json);
    }

    public void Load(){
        if(File.Exists(savePath)){
            string json = File.ReadAllText(savePath);
            CurrentData = JsonUtility.FromJson<GameSaveData>(json);
        }
        else CurrentData = new GameSaveData();
    }

    public void DeleteSave(){
        if(File.Exists(savePath)){
            File.Delete(savePath);
            CurrentData = new GameSaveData();
        }
    }
}