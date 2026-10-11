using UnityEngine;
using System.Collections;
using Nova;

public class SlidingPanel : MonoBehaviour
{
    [SerializeField] private UIBlock2D panel;
    [SerializeField] private Vector2 hiddenPos;
    [SerializeField] private Vector2 shownPos;
    [Space(10)]
    [SerializeField] private float enterDuration = 0.5f;
    [SerializeField] private float exitDuration = 0.5f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Coroutine currentSlideCoroutine;
    private bool isSliding = false;
    private Vector2 currentTarget;

    public void ShowPanel(){
        if(isSliding && currentTarget == shownPos) return;
        if(!isSliding && panel.Position.X.Value == shownPos.x && panel.Position.Y.Value == shownPos.y) return;
        
        if(currentSlideCoroutine != null) StopCoroutine(currentSlideCoroutine);
        currentTarget = shownPos;
        currentSlideCoroutine = StartCoroutine(SlidePanel(shownPos, enterDuration));
    }

    public void HidePanel(){
        if(isSliding && currentTarget == hiddenPos) return;
        if(!isSliding && panel.Position.X.Value == hiddenPos.x && panel.Position.Y.Value == hiddenPos.y) return;
        
        if(currentSlideCoroutine != null) StopCoroutine(currentSlideCoroutine);
        currentTarget = hiddenPos;
        currentSlideCoroutine = StartCoroutine(SlidePanel(hiddenPos, exitDuration));
    }

    IEnumerator SlidePanel(Vector2 targetPos, float duration){
        isSliding = true;
        Vector2 startPos = new Vector2(panel.Position.X.Value, panel.Position.Y.Value);
        float elapsed = 0f;

        while(elapsed < duration){
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(Mathf.Clamp01(elapsed / duration));
            
            float x = Mathf.Lerp(startPos.x, targetPos.x, t);
            float y = Mathf.Lerp(startPos.y, targetPos.y, t);
            
            panel.Position.X.Value = x;
            panel.Position.Y.Value = y;
            
            yield return null;
        }

        panel.Position.X.Value = targetPos.x;
        panel.Position.Y.Value = targetPos.y;
        
        isSliding = false;
        currentSlideCoroutine = null;
    }

    public void SnapToHidden(){
        if(currentSlideCoroutine != null){
            StopCoroutine(currentSlideCoroutine);
            currentSlideCoroutine = null;
        }
        isSliding = false;
        panel.Position.X.Value = hiddenPos.x;
        panel.Position.Y.Value = hiddenPos.y;
    }

    public void SnapToShown(){
        if(currentSlideCoroutine != null){
            StopCoroutine(currentSlideCoroutine);
            currentSlideCoroutine = null;
        }
        isSliding = false;
        panel.Position.X.Value = shownPos.x;
        panel.Position.Y.Value = shownPos.y;
    }

    public bool IsShown() => panel.Position.X.Value == shownPos.x && panel.Position.Y.Value == shownPos.y;
    public bool IsHidden() => panel.Position.X.Value == hiddenPos.x && panel.Position.Y.Value == hiddenPos.y;
    public bool IsSliding() => isSliding;
}