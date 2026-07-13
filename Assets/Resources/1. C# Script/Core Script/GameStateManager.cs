using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    MainMenu,
    Playing,
    Shop,
    Paused
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("UI ROOT")]
    [SerializeField] private GameObject mainMenuRoot;
    [SerializeField] private GameObject gameRoot;
    [SerializeField] private GameObject shopRoot;

    [Header("SCENE NAMES")]
    [ReadOnly] public string mainMenuScene = "MainMenuScene";
    [ReadOnly] public string gameScene = "GameScene";
    [ReadOnly] public string shopScene = "ShopScene";

    [Header("REFERENCES")]
    [SerializeField] private AdditiveSceneManager asm;
    [ReadOnly] public GameState CurrentState;

    public System.Action<GameState, GameState> OnStateChanged;

    private GameState stateBeforePause;

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
        asm.LoadScene(gameScene);
        asm.LoadScene(shopScene);
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(InitializeUI());
    }

    IEnumerator InitializeUI(){
        yield return new WaitUntil(() =>
            SceneManager.GetSceneByName(mainMenuScene).isLoaded &&
            SceneManager.GetSceneByName(gameScene).isLoaded &&
            SceneManager.GetSceneByName(shopScene).isLoaded
        );

        CacheRootObjects();
        ShowMainMenu();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        CacheRootObjects();
    }

    void CacheRootObjects(){
        if(mainMenuRoot == null){
            Scene menuScene = SceneManager.GetSceneByName(mainMenuScene);
            if(menuScene.isLoaded){
                GameObject[] roots = menuScene.GetRootGameObjects();
                if(roots.Length > 0) mainMenuRoot = roots[0];
            }
        }

        if(gameRoot == null){
            Scene gameSceneRef = SceneManager.GetSceneByName(gameScene);
            if(gameSceneRef.isLoaded){
                GameObject[] roots = gameSceneRef.GetRootGameObjects();
                if(roots.Length > 0) gameRoot = roots[0];
            }
        }

        if(shopRoot == null){
            Scene shopSceneRef = SceneManager.GetSceneByName(shopScene);
            if(shopSceneRef.isLoaded){
                GameObject[] roots = shopSceneRef.GetRootGameObjects();
                if(roots.Length > 0) shopRoot = roots[0];
            }
        }
    }

    void SetState(GameState newState){
        if(CurrentState == newState) return;

        GameState previousState = CurrentState;
        CurrentState = newState;
        OnStateChanged?.Invoke(previousState, newState);
    }

    void HideAll(){
        if(mainMenuRoot != null) mainMenuRoot.SetActive(false);
        if(gameRoot != null) gameRoot.SetActive(false);
        if(shopRoot != null) shopRoot.SetActive(false);
    }

    public void ShowMainMenu(){
        HideAll();
        if(mainMenuRoot != null) mainMenuRoot.SetActive(true);
        asm.SetActiveScene(mainMenuScene);
        Time.timeScale = 1f;
        SetState(GameState.MainMenu);
    }

    public void StartGame(){
        HideAll();
        if(gameRoot != null) gameRoot.SetActive(true);
        asm.SetActiveScene(gameScene);
        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void OpenShop(){
        HideAll();
        if(shopRoot != null) shopRoot.SetActive(true);
        asm.SetActiveScene(shopScene);
        Time.timeScale = 1f;
        SetState(GameState.Shop);
    }

    public void CloseShop(){
        HideAll();
        if(gameRoot != null) gameRoot.SetActive(true);
        asm.SetActiveScene(gameScene);
        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void PauseGame(){
        if(CurrentState == GameState.MainMenu || CurrentState == GameState.Paused) return;

        stateBeforePause = CurrentState;
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame(){
        if(CurrentState != GameState.Paused) return;

        Time.timeScale = 1f;
        SetState(stateBeforePause);
    }

    public void TogglePause(){
        if(CurrentState != GameState.Paused && CurrentState != GameState.MainMenu) PauseGame();
        else if(CurrentState == GameState.Paused) ResumeGame();
    }

    public void ReturnToMainMenu(){
        ShowMainMenu();
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    #region INSPECTOR BUTTON
    [Button("Load Main Menu", EButtonEnableMode.Playmode)]
    public void LoadMainMenu() => asm.LoadScene(mainMenuScene);
    [Button("Load Game", EButtonEnableMode.Playmode)]
    public void LoadGame() => asm.LoadScene(gameScene);
    [Button("Load Shop", EButtonEnableMode.Playmode)]
    public void LoadShop() => asm.LoadScene(shopScene);
    #endregion
}