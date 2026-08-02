using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance { get; private set; }
    [SerializeField] private Light2D globalLight;

    private const float MIN_VALUE = 0.25f;
    private const float MAX_VALUE = 1.25f;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLight(float value) => globalLight.intensity = Mathf.Clamp(value, MIN_VALUE, MAX_VALUE);
}