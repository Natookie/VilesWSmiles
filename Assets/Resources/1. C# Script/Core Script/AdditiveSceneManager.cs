using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AdditiveSceneManager : MonoBehaviour
{
    public static AdditiveSceneManager Instance { get; private set; }
    private HashSet<string> loadedScenes = new HashSet<string>();
    private Dictionary<string, AsyncOperation> activeLoads = new Dictionary<string, AsyncOperation>();

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

    public IEnumerator LoadSceneCoroutine(string sceneName){
        if(SceneManager.GetSceneByName(sceneName).isLoaded){
            loadedScenes.Add(sceneName);
            yield break;
        }

        if(activeLoads.ContainsKey(sceneName)){
            yield return new WaitUntil(() => !activeLoads.ContainsKey(sceneName));
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        if(operation == null){
            yield break;
        }

        activeLoads[sceneName] = operation;
        yield return operation;
        activeLoads.Remove(sceneName);

        if(SceneManager.GetSceneByName(sceneName).isLoaded){
            loadedScenes.Add(sceneName);
        }
        else{
            loadedScenes.Remove(sceneName);
        }
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