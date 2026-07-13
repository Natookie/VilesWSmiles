using UnityEngine;
using System.Collections;
using Nova;
using UnityEngine.Events;
using NaughtyAttributes;

public class MybuttonScript : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private ItemView view;
    [SerializeField] private string labelContent;

    [Header("EVENT")]
    [SerializeField] private UnityEvent onClick = new UnityEvent();
    [SerializeField] private bool waitForAnimation = true;

    private MyButtonVisual vis;
    private Transform contentTransform;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Coroutine scaleRoutine;
    private Coroutine hoverRoutine;
    private Coroutine clickRoutine;
    bool hasIcon;

    void Awake() => SyncButton();
    void OnEnable() => StartCoroutine(ResetToDefaultState());
    void OnDisable() => StopAllRoutine();

    void StopAllRoutine(){
        if(clickRoutine != null){
            StopCoroutine(clickRoutine);
            clickRoutine = null;
        }
        if(hoverRoutine != null){
            StopCoroutine(hoverRoutine);
            hoverRoutine = null;
        }
        if(scaleRoutine != null){
            StopCoroutine(scaleRoutine);
            scaleRoutine = null;
        }
    }

    IEnumerator ResetToDefaultState(){
        if(vis == null) yield return null;
        yield return new WaitForEndOfFrame();

        contentTransform.localScale = originalScale;
        contentTransform.localPosition = originalPosition;
        vis.content.Color = vis.content_unhoverColor;
        vis.label.Color = vis.label_unhoverColor;
        if(hasIcon) vis.icon.Color = vis.icon_unhoverColor;
    }

    void HandleClicked(Gesture.OnClick evt){
        if(clickRoutine != null) StopCoroutine(clickRoutine);
        clickRoutine = StartCoroutine(AnimateClick());
    }

    void HandleHover(Gesture.OnHover evt){
        if(hoverRoutine != null) StopCoroutine(hoverRoutine);
        hoverRoutine = StartCoroutine(AnimateHoverIn());
    }

    void HandleUnhover(Gesture.OnUnhover evt){
        if(hoverRoutine != null) StopCoroutine(hoverRoutine);
        hoverRoutine = StartCoroutine(AnimateHoverOut());
    }

    IEnumerator AnimateClick(){
        float elapsed = 0f;
        float sDuration = vis.scaleDuration;
        float cDuration = vis.colorDuration;
        AnimationCurve sCurve = vis.scaleCurve;
        AnimationCurve cCurve = vis.colorCurve;
        Vector3 startScale = contentTransform.localScale;
        Vector3 targetScale = originalScale * vis.clickScale;

        Color contentStartColor = vis.content.Color;
        Color contentTargetColor = vis.content_clickedColor;
        Color labelStartColor = vis.label.Color;
        Color labelTargetColor = vis.label_clickedColor;
        Color iconStartColor = Color.white;
        Color iconTargetColor = Color.white;
        if(hasIcon){
            iconStartColor = vis.icon.Color;
            iconTargetColor = vis.icon_clickedColor;
        }

        if(!waitForAnimation) onClick.Invoke();

        while(elapsed < sDuration || elapsed < cDuration){
            elapsed += Time.deltaTime;

            float sT = sCurve.Evaluate(Mathf.Clamp01(elapsed / sDuration));
            contentTransform.localScale = Vector3.Lerp(startScale, targetScale, sT);

            float cT = cCurve.Evaluate(Mathf.Clamp01(elapsed / cDuration));
            vis.content.Color = Color.Lerp(contentStartColor, contentTargetColor, cT);
            vis.label.Color = Color.Lerp(labelStartColor, labelTargetColor, cT);
            if(hasIcon) vis.icon.Color = Color.Lerp(iconStartColor, iconTargetColor, cT);

            yield return null;
        }

        contentTransform.localScale = targetScale;
        vis.content.Color = contentTargetColor;
        vis.label.Color = labelTargetColor;
        if(hasIcon) vis.icon.Color = iconTargetColor;

        elapsed = 0f;
        startScale = contentTransform.localScale;
        contentStartColor = vis.content.Color;
        labelStartColor = vis.label.Color;
        if(hasIcon) iconStartColor = vis.icon.Color;
        while(elapsed < sDuration || elapsed < cDuration){
            elapsed += Time.deltaTime;

            float sT = sCurve.Evaluate(Mathf.Clamp01(elapsed / sDuration));
            contentTransform.localScale = Vector3.Lerp(startScale, originalScale, sT);

            float cT = cCurve.Evaluate(Mathf.Clamp01(elapsed / cDuration));
            vis.content.Color = Color.Lerp(contentStartColor, vis.content_unhoverColor, cT);
            vis.label.Color = Color.Lerp(labelStartColor, vis.label_unhoverColor, cT);
            if(hasIcon) vis.icon.Color = Color.Lerp(iconStartColor, vis.icon_unhoverColor, cT);

            yield return null;
        }

        contentTransform.localScale = originalScale;
        vis.content.Color = vis.content_unhoverColor;
        vis.label.Color = vis.label_unhoverColor;
        if(hasIcon) vis.icon.Color = vis.icon_unhoverColor;

        if(waitForAnimation){
            yield return new WaitForSeconds(vis.scaleDuration);
            onClick.Invoke();
        }
    }

    IEnumerator AnimateHoverIn(){
        float elapsed = 0f;
        float hDuration = vis.hoverDuration;
        float cDuration = vis.colorDuration;
        AnimationCurve hCurve = vis.hoverCurve;
        AnimationCurve cCurve = vis.colorCurve;
        Vector3 startPosition = contentTransform.localPosition;
        Vector3 targetPosition = originalPosition + (Vector3)vis.hoverVector;

        Color contentStartColor = vis.content.Color;
        Color contentTargetColor = vis.content_hoverColor;
        Color labelStartColor = vis.label.Color;
        Color labelTargetColor = vis.label_hoverColor;
        Color iconStartColor = Color.white;
        Color iconTargetColor = Color.white;
        if(hasIcon){
            iconStartColor = vis.icon.Color;
            iconTargetColor = vis.icon_hoverColor;
        }

        while(elapsed < hDuration || elapsed < cDuration){
            elapsed += Time.deltaTime;

            float hT = hCurve.Evaluate(Mathf.Clamp01(elapsed / hDuration));
            contentTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, hT);

            float cT = cCurve.Evaluate(Mathf.Clamp01(elapsed / cDuration));
            vis.content.Color = Color.Lerp(contentStartColor, contentTargetColor, cT);
            vis.label.Color = Color.Lerp(labelStartColor, labelTargetColor, cT);
            if(hasIcon) vis.icon.Color = Color.Lerp(iconStartColor, iconTargetColor, cT);

            yield return null;
        }

        contentTransform.localPosition = targetPosition;
        vis.content.Color = contentTargetColor;
        vis.label.Color = labelTargetColor;
        if(hasIcon) vis.icon.Color = iconTargetColor;
    }

    IEnumerator AnimateHoverOut(){
        float elapsed = 0f;
        float hDuration = vis.hoverDuration;
        float cDuration = vis.colorDuration;
        AnimationCurve hCurve = vis.hoverCurve;
        AnimationCurve cCurve = vis.colorCurve;
        Vector3 startPosition = contentTransform.localPosition;

        Color contentStartColor = vis.content.Color;
        Color contentTargetColor = vis.content_unhoverColor;
        Color labelStartColor = vis.label.Color;
        Color labelTargetColor = vis.label_unhoverColor;
        Color iconStartColor = Color.white;
        Color iconTargetColor = Color.white;
        if(hasIcon){
            iconStartColor = vis.icon.Color;
            iconTargetColor = vis.icon_unhoverColor;
        }

        while(elapsed < hDuration || elapsed < cDuration){
            elapsed += Time.deltaTime;

            float hT = hCurve.Evaluate(Mathf.Clamp01(elapsed / hDuration));
            contentTransform.localPosition = Vector3.Lerp(startPosition, originalPosition, hT);

            float cT = cCurve.Evaluate(Mathf.Clamp01(elapsed / cDuration));
            vis.content.Color = Color.Lerp(contentStartColor, contentTargetColor, cT);
            vis.label.Color = Color.Lerp(labelStartColor, labelTargetColor, cT);
            if(hasIcon) vis.icon.Color = Color.Lerp(iconStartColor, iconTargetColor, cT);

            yield return null;
        }

        contentTransform.localPosition = originalPosition;
        vis.content.Color = contentTargetColor;
        vis.label.Color = labelTargetColor;
        if(hasIcon) vis.icon.Color = iconTargetColor;
    }

    [Button("Sync Button", EButtonEnableMode.Editor)]
    public void SyncButton(){
        vis = view.Visuals as MyButtonVisual;
        vis.container.AddGestureHandler<Gesture.OnClick>(HandleClicked);
        vis.container.AddGestureHandler<Gesture.OnHover>(HandleHover);
        vis.container.AddGestureHandler<Gesture.OnUnhover>(HandleUnhover);

        contentTransform = vis.content.transform;
        originalScale = contentTransform.localScale;
        originalPosition = contentTransform.localPosition;
        
        vis.label.Text = labelContent;
        vis.content.Color = vis.content_unhoverColor;
        vis.label.Color = vis.label_unhoverColor;

        hasIcon = vis.icon != null;
        if(hasIcon) vis.icon.Color = vis.icon_unhoverColor;
    }
}