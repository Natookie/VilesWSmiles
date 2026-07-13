using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AdditiveSceneManager : MonoBehaviour
{
    public static AdditiveSceneManager Instance { get; private set; }
    private HashSet<string> loadedScenes = new HashSet<string>();
    private HashSet<string> loadingScenes = new HashSet<string>();

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SyncWithUnityScenes();
    }

    void SyncWithUnityScenes(){
        for(int i = 0; i < SceneManager.sceneCount; i++){
            Scene scene = SceneManager.GetSceneAt(i);
            loadedScenes.Add(scene.name);
        }
    }

    public AsyncOperation LoadScene(string sceneName){
        if(SceneManager.GetSceneByName(sceneName).isLoaded){
            loadedScenes.Add(sceneName);
            return null;
        }

        if(loadingScenes.Contains(sceneName)) return null;
        loadingScenes.Add(sceneName);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if(operation == null){
            loadingScenes.Remove(sceneName);
            return null;
        }

        operation.completed += (op) =>{
            loadingScenes.Remove(sceneName);

            if(SceneManager.GetSceneByName(sceneName).isLoaded) loadedScenes.Add(sceneName);
            else loadedScenes.Remove(sceneName);
        };

        return operation;
    }

    public void UnloadScene(string sceneName){
        if(!SceneManager.GetSceneByName(sceneName).isLoaded){
            loadedScenes.Remove(sceneName);
            return;
        }

        SceneManager.UnloadSceneAsync(sceneName).completed += (op) =>{
            loadedScenes.Remove(sceneName);
        };
    }

    public void SetActiveScene(string sceneName){
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if(scene.isLoaded) SceneManager.SetActiveScene(scene);
    }

    public bool IsSceneLoaded(string sceneName) => SceneManager.GetSceneByName(sceneName).isLoaded;
}