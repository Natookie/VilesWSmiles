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
        if(Keyboard.current.escapeKey.wasPressedThisFrame) GameStateManager.Instance.TogglePause();

        if(!enableDebugInput) return;

        //Game State Manager
        // if(Keyboard.current.spaceKey.wasPressedThisFrame) GameStateManager.Instance.ShowMainMenu();
    }
}