using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    MainMenu,
    Playing,
    Shop,
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

    private GameState previousState;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        StartCoroutine(InitializeUI());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator InitializeUI(){
        yield return StartCoroutine(asm.LoadSceneCoroutine(mainMenuScene));
        yield return StartCoroutine(asm.LoadSceneCoroutine(gameScene));
        yield return StartCoroutine(asm.LoadSceneCoroutine(shopScene));
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
                foreach(GameObject root in roots){
                    if(root.CompareTag("MainMenuUI")){
                        mainMenuRoot = root;
                        break;
                    }
                }
                if(mainMenuRoot == null && roots.Length > 0) mainMenuRoot = roots[0];
            }
        }

        if(gameRoot == null){
            Scene gameSceneRef = SceneManager.GetSceneByName(gameScene);
            if(gameSceneRef.isLoaded){
                GameObject[] roots = gameSceneRef.GetRootGameObjects();
                foreach(GameObject root in roots){
                    if(root.CompareTag("GameUI")){
                        gameRoot = root;
                        break;
                    }
                }
                if(gameRoot == null && roots.Length > 0) gameRoot = roots[0];
            }
        }

        if(shopRoot == null){
            Scene shopSceneRef = SceneManager.GetSceneByName(shopScene);
            if(shopSceneRef.isLoaded){
                GameObject[] roots = shopSceneRef.GetRootGameObjects();
                foreach(GameObject root in roots){
                    if(root.CompareTag("ShopUI")){
                        shopRoot = root;
                        break;
                    }
                }
                if(shopRoot == null && roots.Length > 0) shopRoot = roots[0];
            }
        }
    }

    void SetState(GameState newState){
        if(CurrentState == newState) return;
        previousState = CurrentState;
        GameState oldState = CurrentState;
        CurrentState = newState;
        OnStateChanged?.Invoke(oldState, newState);
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
        SetState(GameState.MainMenu);
    }

    public void StartGame(){
        HideAll();
        if(gameRoot != null) gameRoot.SetActive(true);
        asm.SetActiveScene(gameScene);
        SetState(GameState.Playing);
    }

    public void OpenRecipe(){
        previousState = CurrentState;
        HideAll();
        if(shopRoot != null) shopRoot.SetActive(true);
        asm.SetActiveScene(shopScene);
        SetState(GameState.Shop);
        AudioManager.Instance.PlayUI(UISound.PanelOpen);
    }

    public void CloseShop(){
        switch(previousState){
            case GameState.Playing:
                StartGame();
                break;
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            default:
                ShowMainMenu();
                break;
        }
    }

    public void TogglePause(){
        if(CurrentState == GameState.Shop){
            CloseShop();
            AudioManager.Instance.PlayUI(UISound.PanelOpen);
            DialogueManager.Instance.TriggerInteraction("CloseRecipeBook");
            return;
        }
        
        if(CurrentState == GameState.Playing){
            if(DialogueManager.Instance.IsDialogueVisible()) return;
            ShowMainMenu();
            AudioManager.Instance.PlayUI(UISound.PanelOpen);
            return;
        }
        
        if(CurrentState == GameState.MainMenu){
            StartGame();
            AudioManager.Instance.PlayUI(UISound.PanelOpen);
            return;
        }
    }

    public void ReturnToMainMenu() => ShowMainMenu();
    
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
    public void LoadMainMenu() => StartCoroutine(asm.LoadSceneCoroutine(mainMenuScene));
    [Button("Load Game", EButtonEnableMode.Playmode)]
    public void LoadGame() => StartCoroutine(asm.LoadSceneCoroutine(gameScene));
    [Button("Load Shop", EButtonEnableMode.Playmode)]
    public void LoadShop() => StartCoroutine(asm.LoadSceneCoroutine(shopScene));
    #endregion
}