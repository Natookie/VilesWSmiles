using UnityEngine;
using UnityEngine.InputSystem;

public class Essential : MonoBehaviour
{
    [Header("DEBUG")]
    [SerializeField] private bool enableDebugInput = false;

    void Start(){
        #if !UNITY_EDITOR
            enableDebugInput = false;
        #else 
            enableDebugInput = true;
        #endif
    }

    void Update(){
        HandleDebugInput();
    }

    void HandleDebugInput(){
        if(!enableDebugInput) return;

        //Game State Manager
        if(Keyboard.current.spaceKey.wasPressedThisFrame) GameStateManager.Instance.ShowMainMenu();
        if(Keyboard.current.escapeKey.wasPressedThisFrame) GameStateManager.Instance.TogglePause();

        //Audio Manager
        if(Keyboard.current.digit1Key.wasPressedThisFrame){
            float masterVolume = AudioManager.Instance.GetMasterVolume();
            AudioManager.Instance.SetMasterVolume(masterVolume == 0f ? 1f : 0f);
        }

        if(Keyboard.current.digit2Key.wasPressedThisFrame){
            float bgmVolume = AudioManager.Instance.GetBGMVolume();
            AudioManager.Instance.SetBGMVolume(bgmVolume == 0f ? 1f : 0f);
        }

        if(Keyboard.current.iKey.wasPressedThisFrame) AudioManager.Instance.PlayBGM(BGMTrack.MainMenu);
        if(Keyboard.current.oKey.wasPressedThisFrame) AudioManager.Instance.PlaySFX(SFXSound.SFXText);
    }
}