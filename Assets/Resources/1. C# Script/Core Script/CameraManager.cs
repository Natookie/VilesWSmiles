using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.Mathematics;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance {get; private set;}

    [Header("REFERENCES")]
    [SerializeField] private CinemachineCamera thisCam;
    private CinemachineBasicMultiChannelPerlin noise;
    
    [Header("TWEAK")]
    [SerializeField] private Vector2 zoomConstraint;
    [SerializeField] private float zoomSensitivity;

    private float cachedZoomValue;
    private Transform player;
    private bool isPlaying;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        GameStateManager.Instance.OnStateChanged += OnStateChanged;
        noise = thisCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void OnEnable(){if(noise != null) noise.AmplitudeGain = 0f;}

    void OnStateChanged(GameState previousState, GameState newState){
        if(newState == GameState.Playing){
            if(player == null){
                player = FindFirstObjectByType<PlayerController>().transform;
                thisCam.Target.TrackingTarget = player;
            }
            isPlaying = true;
        }
        else isPlaying = false;
    }

    void Update(){
        HandleZoom();
    }

    void HandleZoom(){
        if(!isPlaying) return;
        
        Mouse mouse = Mouse.current;
        if(mouse == null) return;
        
        float scrollInput = mouse.scroll.y.ReadValue();
        if(Mathf.Approximately(scrollInput, 0f)) return;
        
        cachedZoomValue = thisCam.Lens.OrthographicSize;
        cachedZoomValue -= scrollInput * zoomSensitivity * Time.deltaTime;
        cachedZoomValue = Mathf.Clamp(cachedZoomValue, zoomConstraint.x, zoomConstraint.y);
        
        thisCam.Lens.OrthographicSize = Mathf.Lerp(thisCam.Lens.OrthographicSize, cachedZoomValue, Time.deltaTime * 10f);
    }

    public void ShakeCamera(float intensity = 1f, float duration = .55f) => StartCoroutine(ShakeRoutine(intensity, duration));
    IEnumerator ShakeRoutine(float intensity, float duration){
        if(noise != null){
            noise.AmplitudeGain = intensity;
            yield return new WaitForSeconds(duration);
            noise.AmplitudeGain = 0;
        }
    }
}