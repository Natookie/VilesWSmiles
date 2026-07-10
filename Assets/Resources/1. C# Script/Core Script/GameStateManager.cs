using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("UI ROOT")]
    [SerializeField] private GameObject mainMenuRoot;

    [Header("SCENE NAMES")]
    [ReadOnly] public string mainMenuScene = "MainMenuScene";
    [ReadOnly] public string gameScene = "GameScene";

    [Header("REFERENCES")]
    [SerializeField] private AdditiveSceneManager asm;

    private string currentLevelScene;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        asm.LoadScene(mainMenuScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(InitializeUI());
    }

    IEnumerator InitializeUI(){
        yield return new WaitUntil(() =>
            SceneManager.GetSceneByName(mainMenuScene).isLoaded
        );

        CacheMainMenuRoot();

        if(mainMenuRoot != null) mainMenuRoot.SetActive(true);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == mainMenuScene){
            CacheMainMenuRoot();
        }
    }

    void CacheMainMenuRoot(){
        if(mainMenuRoot == null){
            Scene menuScene = SceneManager.GetSceneByName(mainMenuScene);
            if(menuScene.isLoaded){
                GameObject[] roots = menuScene.GetRootGameObjects();
                if(roots.Length > 0) mainMenuRoot = roots[0];
            }
        }
    }

    #region START GAME
    public void StartGame(string levelScene = null) => StartCoroutine(StartGameRoutine(levelScene));
    IEnumerator StartGameRoutine(string levelScene){
        string level = string.IsNullOrEmpty(levelScene) ? gameScene : levelScene;

        if(mainMenuRoot != null) mainMenuRoot.SetActive(false);
        AsyncOperation operation = asm.LoadScene(level);

        if(operation != null) yield return operation;
        if(asm.IsSceneLoaded(level)){
            asm.SetActiveScene(level);
            currentLevelScene = level;
        }
    }
    #endregion

    #region END GAME
    public void EndGame() => StartCoroutine(EndGameRoutine());
    IEnumerator EndGameRoutine(){
        if(!string.IsNullOrEmpty(currentLevelScene)){
            asm.UnloadScene(currentLevelScene);
            currentLevelScene = null;
        }

        yield return null;
        if(mainMenuRoot != null) mainMenuRoot.SetActive(true);
        Time.timeScale = 1f;
    }
    #endregion

    #region SWITCH LEVEL
    public void SwitchLevel(string newLevelScene) => StartCoroutine(SwitchLevelRoutine(newLevelScene));
    IEnumerator SwitchLevelRoutine(string newLevelScene){
        if(!string.IsNullOrEmpty(currentLevelScene)) asm.UnloadScene(currentLevelScene);
        yield return null;

        AsyncOperation operation = asm.LoadScene(newLevelScene);
        if(operation != null) yield return operation;

        if(asm.IsSceneLoaded(newLevelScene)){
            asm.SetActiveScene(newLevelScene);
            currentLevelScene = newLevelScene;
        }
    }
    #endregion

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    #region INSPECTOR BUTTON
    [Button("Load Main Menu", EButtonEnableMode.Playmode)]
    public void LoadMainMenu() => asm.LoadScene(mainMenuScene);
    [Button("Load Game", EButtonEnableMode.Playmode)]
    public void LoadGame() => asm.LoadScene(gameScene);
    #endregion
}