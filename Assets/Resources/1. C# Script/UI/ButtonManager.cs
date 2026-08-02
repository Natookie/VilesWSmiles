using UnityEngine;
using Nova;
using NaughtyAttributes;

public class ButtonManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [ReadOnly] public AdditiveSceneManager asm;

    void Awake() => asm ??= AdditiveSceneManager.Instance;

    #region PUBLIC FUNCTION
    //Main Menus
    public void Evt_AddGameScene() => GameStateManager.Instance.StartGame();
    public void Evt_AddShopScene() => GameStateManager.Instance.OpenRecipe();
    public void Evt_ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Setting
    public void SwitchLanguage(){
        LocalizationManager.Instance.SetLanguage((LocalizationManager.Instance.GetCurrentLanguageIndex() + 1) % LocalizationManager.Instance.GetSupportedLanguages().Length);
        SaveManager.Instance.Save();
    }
    public void ToggleFullScreen(){
        bool fullscreen = !SaveManager.Instance.CurrentData.useFullScreen;
        SaveManager.Instance.CurrentData.useFullScreen = fullscreen;
        SaveManager.Instance.Save();

        Screen.fullScreenMode = fullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;
    }
    public void DeleteData(){
        SaveManager.Instance.DeleteSave();
        SaveManager.Instance.Load();
    }
    public void SkipTutorial(){
        SaveManager.Instance.CurrentData.hasCompletedTutorial = true;
        SaveManager.Instance.Save();
    }
    #endregion
}
