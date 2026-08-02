using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using NaughtyAttributes;
using Nova;

public class GameFlowManager : MonoBehaviour
{
    [ReadOnly] public bool introChecked;

    [SerializeField] private UIBlock2D blackPanel;
    [SerializeField] private GameObject finalSaying;
    [SerializeField] private Color panelColor;
    private GameObject clickableSystem;

    void Start(){
        GameStateManager.Instance.OnStateChanged += OnStateChanged;
        finalSaying.SetActive(false);
    }

    void OnStateChanged(GameState previousState, GameState newState){
        if(newState == GameState.Playing){
            if(introChecked) return;
            introChecked = true;
            if(!SaveManager.Instance.CurrentData.hasCompletedTutorial){
                if(clickableSystem == null) clickableSystem = FindFirstObjectByType<RecipeUI>().gameObject;
                StartCoroutine(PlayIntro());
            }
        }
        else if(newState == GameState.MainMenu){
            if(SaveManager.Instance.CurrentData.hasFinishedGame){
                StartCoroutine(PlayOutro());
            }
        }
    }

    IEnumerator PlayIntro(){
        clickableSystem.SetActive(false);
        Color color = panelColor;
        color.a = 1f;
        blackPanel.Color = color;
        yield return StartCoroutine(DialogueLibrary.Instance.PlayIntroSequence());
        clickableSystem.SetActive(true);
        float duration = 2f;
        float elapsed = 0f;
        Color startColor = blackPanel.Color;
        Color endColor = new Color(panelColor.r, panelColor.g, panelColor.b, 0f);
        
        while(elapsed < duration){
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            blackPanel.Color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        blackPanel.Color = endColor;
        yield return StartCoroutine(DialogueLibrary.Instance.PlayTutorialSequence());
        SaveManager.Instance.CurrentData.hasCompletedTutorial = true;
        SaveManager.Instance.Save();
        CustomerManager.Instance.StartFirstShift();
    }

    IEnumerator PlayOutro(){
        clickableSystem.SetActive(false);
        Color startColor = blackPanel.Color;
        Color endColor = panelColor;
        float duration = 3f;
        float elapsed = 0f;
        
        while(elapsed < duration){
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            blackPanel.Color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        
        blackPanel.Color = endColor;
        yield return new WaitForSeconds(3f);
        finalSaying.SetActive(true);
        yield return new WaitUntil(() => Keyboard.current.anyKey.wasPressedThisFrame);
        Debug.Log("gone");
        SaveManager.Instance.DeleteSave();
        clickableSystem.SetActive(true);
        Application.Quit();
    }

    void OnDestroy(){
        GameStateManager.Instance.OnStateChanged -= OnStateChanged;
    }
}