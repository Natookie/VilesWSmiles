using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("INPUT ACTIONS")]
    [SerializeField] private InputActionAsset inputActions;

    public float MoveInput { get; private set; }

    public static System.Action OnInteract;
    public static System.Action OnPause;
    public static System.Action OnInventory;
    public static System.Action<KeyCode> OnKeyPressed;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction pauseAction;
    private InputAction inventoryAction;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupInputActions();
    }

    void SetupInputActions(){
        var playerMap = inputActions.FindActionMap("Player");
        var uiMap = inputActions.FindActionMap("UI");

        moveAction = playerMap.FindAction("Move");
        interactAction = playerMap.FindAction("Interact");
        pauseAction = uiMap.FindAction("Pause");
        inventoryAction = uiMap.FindAction("Inventory");

        moveAction.performed += ctx => MoveInput = ctx.ReadValue<Vector2>().x;
        moveAction.canceled += ctx => MoveInput = 0f;

        interactAction.performed += ctx => OnInteract?.Invoke();
        pauseAction.performed += ctx => OnPause?.Invoke();
        inventoryAction.performed += ctx => OnInventory?.Invoke();
    }

    void Update(){
        if(Keyboard.current == null) return;

        if(Keyboard.current.aKey.wasPressedThisFrame) OnKeyPressed?.Invoke(KeyCode.A);
        if(Keyboard.current.dKey.wasPressedThisFrame) OnKeyPressed?.Invoke(KeyCode.D);
        if(Keyboard.current.fKey.wasPressedThisFrame) OnKeyPressed?.Invoke(KeyCode.F);
        if(Keyboard.current.eKey.wasPressedThisFrame) OnKeyPressed?.Invoke(KeyCode.E);
        if(Keyboard.current.escapeKey.wasPressedThisFrame) OnKeyPressed?.Invoke(KeyCode.Escape);
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable(){
        inputActions.Disable();
        MoveInput = 0f;
    }
}