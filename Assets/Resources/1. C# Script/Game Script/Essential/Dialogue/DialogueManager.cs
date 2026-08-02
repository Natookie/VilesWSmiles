using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("REFERENCES")]
    [SerializeField] private DialogueUI dialogueUI;

    private bool isPlaying;
    private bool conditionMet;
    private float holdTimer;
    private string waitedEventId;
    private SpeakerType currentSpeaker;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayDialogue(SpeakerType speaker, string localizationKey, params object[] args){
        string text = LocalizationManager.Instance.GetText(localizationKey);
        if(args.Length > 0) text = string.Format(text, args);
        currentSpeaker = speaker;
        dialogueUI.ShowDialogue(speaker, text);
    }

    void PlayDialogueWithCustomName(string speakerName, SpeakerType speaker, string localizationKey, params object[] args){
        string text = LocalizationManager.Instance.GetText(localizationKey);
        if(args.Length > 0) text = string.Format(text, args);
        currentSpeaker = speaker;
        dialogueUI.ShowDialogueWithCustomName(speakerName, speaker, text);
    }

    public void PlayCustomerReaction(string speakerName, string localizationKey, params object[] args){
        PlayDialogueWithCustomName(speakerName, SpeakerType.Customer, localizationKey, args);
        StartCoroutine(AutoHideRoutine());
    }

    IEnumerator AutoHideRoutine(){
        yield return WaitForSeconds();
        HideDialogue();
    }

    public bool IsSystemSpeaking() => currentSpeaker == SpeakerType.System && dialogueUI.IsVisible();
    public bool IsDialogueVisible() => dialogueUI.IsVisible();
    public bool IsDialogueTyping() => dialogueUI.IsTyping();

    #region API
    public IEnumerator WaitForTyping(){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
    }

    public IEnumerator WaitForSeconds(float seconds = 0.75f){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
        
        float timer = 0f;
        while(timer < seconds){
            // if(Keyboard.current.spaceKey.wasPressedThisFrame) yield break;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator WaitForKeyPress(KeyCode key){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
        conditionMet = false;
        InputManager.OnKeyPressed += CheckKey;
        yield return new WaitUntil(() => conditionMet);
        InputManager.OnKeyPressed -= CheckKey;
    }

    void CheckKey(KeyCode pressedKey) => conditionMet = true;

    public IEnumerator WaitForInteraction(string eventId){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
        waitedEventId = eventId;
        conditionMet = false;
        yield return new WaitUntil(() => conditionMet);
        waitedEventId = null;
    }

    public void TriggerInteraction(string eventId){
        if(waitedEventId == eventId) conditionMet = true;
    }
    #endregion

    #region TUTORIAL
    public IEnumerator WaitForMoveLeft(float duration){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
        holdTimer = 0f;
        yield return new WaitUntil(() => {
            if(InputManager.Instance.MoveInput < 0f) holdTimer += Time.deltaTime;
            else holdTimer = 0f;
            return holdTimer >= duration;
        });
    }

    public IEnumerator WaitForMoveRight(float duration){
        yield return new WaitUntil(() => !dialogueUI.IsTyping());
        holdTimer = 0f;
        yield return new WaitUntil(() => {
            if(InputManager.Instance.MoveInput > 0f) holdTimer += Time.deltaTime;
            else holdTimer = 0f;
            return holdTimer >= duration;
        });
    }
    #endregion

    public void HideDialogue() => dialogueUI.HideDialogue();
    public bool IsTyping() => dialogueUI.IsTyping();
}