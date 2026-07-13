using UnityEngine;
using Nova;
using NaughtyAttributes;

public class ButtonManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [ReadOnly] public AdditiveSceneManager asm;

    void Awake(){
        asm ??= AdditiveSceneManager.Instance;
    }

    #region PUBLIC FUNCTION
    //Main Menus
    public void Evt_AddGameScene() => GameStateManager.Instance.StartGame();
    public void Evt_AddShopScene() => GameStateManager.Instance.OpenShop();
    public void Evt_ExitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Setting
    public void SwitchLanguage() => LocalizationManager.Instance.SetLanguage((LocalizationManager.Instance.GetCurrentLanguageIndex() + 1) % LocalizationManager.Instance.GetSupportedLanguages().Length);
    #endregion
}
