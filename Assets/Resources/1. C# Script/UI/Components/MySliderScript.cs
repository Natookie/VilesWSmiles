using UnityEngine;
using System.Collections;
using Nova;
using UnityEngine.Events;
using NaughtyAttributes;
using NovaSamples.UIControls;

[System.Serializable]
public class SliderValueChangedEvent : UnityEvent<float, string> { }

public class MySliderScript : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private ItemView view;
    [SerializeField] private string labelContent;
    [SerializeField] private MySliderConfig config;
    [Space(10)]
    [Range(0f, 1f)] [SerializeField] private float defaultValue = 0.5f;
    [SerializeField] private float sizeOffset = 10f;

    [Header("EVENT")]
    [SerializeField] private bool needBridge;
    public enum SliderType
    {
        MasterVolume,
        BGMVolume,
        SFXVolume,
        UIVolume,
        Resolution,
        Brightness,
        Default
    }
    [ShowIf("needBridge")][SerializeField] private SliderType type = SliderType.Default;
    [SerializeField] private SliderValueChangedEvent onValueChanged = new SliderValueChangedEvent();

    private MySliderVisual vis;
    private Transform draggableTransform;
    private Vector3 draggableOriginalScale;
    private float fillPercentage;
    private float normalizedValue;
    private float previousFillPercentage;
    private float currentStretchNormal;
    private bool isDragging;
    private Coroutine stretchRoutine;
    private Coroutine snapRoutine;

    void Awake(){
        SyncSlider();
    }
    void Start(){
        SetNormalizedValue(defaultValue);
    }

    void HandlePointerDown(Gesture.OnPress evt){
        isDragging = true;
        previousFillPercentage = fillPercentage;
        if(snapRoutine != null) StopCoroutine(snapRoutine);
        vis.draggable.Color = vis.draggable_clickedColor;
        vis.data.Color = vis.data_clickedColor;
    }

    void HandlePointerUp(Gesture.OnRelease evt){
        isDragging = false;
        vis.draggable.Color = vis.draggable_unhoverColor;
        vis.data.Color = vis.data_unhoverColor;
        RevertStretch();

        if(config.stepCount > 1){
            float step = 1f / (config.stepCount - 1);
            float targetValue = Mathf.Round(normalizedValue / step) * step;
            if(snapRoutine != null) StopCoroutine(snapRoutine);
            snapRoutine = StartCoroutine(SnapToValue(targetValue));
        }
    }

    void HandleHover(Gesture.OnHover evt){
        if(isDragging) return;
        vis.draggable.Color = vis.draggable_hoverColor;
        vis.data.Color = vis.data_hoverColor;
        AudioManager.Instance.PlayUI(UISound.ButtonHover);
    }

    void HandleUnhover(Gesture.OnUnhover evt){
        if(isDragging) return;
        vis.draggable.Color = vis.draggable_unhoverColor;
        vis.data.Color = vis.data_unhoverColor;
    }

    private float lastDragSoundTime;
    private float dragSoundCooldown = 0.05f;

    void HandleDrag(Gesture.OnDrag evt){
        if(!isDragging) return;

        float backgroundWidth = vis.background.CalculatedSize.X.Value;
        float deltaPercent = evt.DragDeltaLocalSpace.x / backgroundWidth;
        float rawFill = fillPercentage + deltaPercent;
        fillPercentage += deltaPercent;
        fillPercentage = Mathf.Clamp(fillPercentage, 0f, 1f);
        normalizedValue = fillPercentage;

        bool clampedAtLimit = (fillPercentage == 0f && rawFill < 0f) || (fillPercentage == 1f && rawFill > 1f);
        float changeDelta = Mathf.Abs(fillPercentage - previousFillPercentage);
        float targetStretchNormal;

        if(clampedAtLimit) targetStretchNormal = 1f;
        else targetStretchNormal = Mathf.Clamp01(changeDelta / 0.2f);

        currentStretchNormal = Mathf.Lerp(currentStretchNormal, targetStretchNormal, 0.3f);

        float stretchAmount = Mathf.Lerp(1f, vis.xStretchScale, currentStretchNormal);
        Vector3 stretchScale = draggableOriginalScale;
        stretchScale.x *= stretchAmount;
        draggableTransform.localScale = stretchScale;

        previousFillPercentage = fillPercentage;

        UpdateFill();
        UpdateDataText();

        if(Time.time - lastDragSoundTime >= dragSoundCooldown){
            AudioManager.Instance.PlayUI(UISound.SliderDrag);
            lastDragSoundTime = Time.time;
        }
    }

    IEnumerator SnapToValue(float targetValue){
        float elapsed = 0f;
        float duration = vis.colorDuration;
        AnimationCurve curve = vis.colorCurve;
        float startValue = normalizedValue;

        while(elapsed < duration){
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(elapsed / duration));
            normalizedValue = Mathf.Lerp(startValue, targetValue, t);
            fillPercentage = normalizedValue;
            UpdateFill();
            UpdateDataText();
            yield return null;
        }

        normalizedValue = targetValue;
        fillPercentage = normalizedValue;
        UpdateFill();
        UpdateDataText();
    }

    void RevertStretch(){
        if(stretchRoutine != null) StopCoroutine(stretchRoutine);
        stretchRoutine = StartCoroutine(RevertStretchRoutine());
    }

    IEnumerator RevertStretchRoutine(){
        float elapsed = 0f;
        float duration = vis.colorDuration;
        AnimationCurve curve = vis.colorCurve;
        Vector3 startScale = draggableTransform.localScale;
        Vector3 targetScale = draggableOriginalScale;

        while(elapsed < duration){
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(elapsed / duration));
            draggableTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        draggableTransform.localScale = targetScale;
        currentStretchNormal = 0f;
    }

    void UpdateFill(){
        Length3 fillSize = vis.fill.Size;
        fillSize.X.Percent = fillPercentage;
        vis.fill.Size = fillSize;
    }

    void UpdateDataText(){
        string valueText = config.GetValueText(normalizedValue);
        vis.data.Text = valueText;

        onValueChanged.Invoke(normalizedValue, valueText);
        switch(type){
            case SliderType.MasterVolume: AudioManager.Instance.SetMasterVolume(normalizedValue); break;
            case SliderType.BGMVolume: AudioManager.Instance.SetBGMVolume(normalizedValue); break;
            case SliderType.SFXVolume: AudioManager.Instance.SetSFXVolume(normalizedValue); break;
            case SliderType.UIVolume: AudioManager.Instance.SetUIVolume(normalizedValue); break;
            case SliderType.Resolution:
                SaveManager.Instance.CurrentData.resolutionIndex = Mathf.RoundToInt(normalizedValue * (config.stepCount - 1));
                SaveManager.Instance.Save();
                break;
            case SliderType.Brightness:
                float brightness = Mathf.Lerp(0.25f, 1.25f, normalizedValue);
                LightManager.Instance.SetLight(brightness);
                SaveManager.Instance.CurrentData.brightness = brightness;
                SaveManager.Instance.Save();
                break;
        }
    }

    public void SetNormalizedValue(float value){
        switch(type){
            case SliderType.MasterVolume: normalizedValue = SaveManager.Instance.CurrentData.masterVolume; break;
            case SliderType.BGMVolume: normalizedValue = SaveManager.Instance.CurrentData.bgmVolume; break;
            case SliderType.SFXVolume: normalizedValue = SaveManager.Instance.CurrentData.sfxVolume; break;
            case SliderType.UIVolume: normalizedValue = SaveManager.Instance.CurrentData.uiVolume; break;
            case SliderType.Resolution: 
                float step = config.stepCount > 1 ? 1f / (config.stepCount - 1) : 1f;
                normalizedValue = SaveManager.Instance.CurrentData.resolutionIndex * step;
                break;
            case SliderType.Brightness:
                float savedBrightness = SaveManager.Instance.CurrentData.brightness;
                normalizedValue = Mathf.InverseLerp(0.25f, 1.25f, savedBrightness);
                break;
            default: normalizedValue = Mathf.Clamp01(value); break;
        }

        fillPercentage = normalizedValue;
        previousFillPercentage = fillPercentage;
        currentStretchNormal = 0f;
        UpdateFill();
        UpdateDataText();
    }

    public float GetNormalizedValue() => normalizedValue;

    [Button("Sync Slider", EButtonEnableMode.Editor)]
    public void SyncSlider(){
        vis = view.Visuals as MySliderVisual;
        vis.draggable.AddGestureHandler<Gesture.OnPress>(HandlePointerDown);
        vis.draggable.AddGestureHandler<Gesture.OnRelease>(HandlePointerUp);
        vis.draggable.AddGestureHandler<Gesture.OnHover>(HandleHover);
        vis.draggable.AddGestureHandler<Gesture.OnUnhover>(HandleUnhover);
        vis.draggable.AddGestureHandler<Gesture.OnDrag>(HandleDrag);

        draggableTransform = vis.draggable.transform;
        draggableOriginalScale = draggableTransform.localScale;
        fillPercentage = normalizedValue;
        previousFillPercentage = fillPercentage;

        vis.label.Text = labelContent;
        vis.background.Color = vis.background_color;
        vis.fill.Color = vis.fill_color;
        vis.draggable.Color = vis.draggable_unhoverColor;
        vis.data.Color = vis.data_unhoverColor;

        vis.draggable.Position.X.Value = -vis.draggable.Size.X.Value + sizeOffset;
    }
}