using UnityEngine;
using Nova;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private UIBlock2D speakerIcon;
    [SerializeField] private TextBlock speakerNameText;
    [SerializeField] private UIBlock2D speakerLine;
    [SerializeField] private TextBlock dialogueText;
    [SerializeField] private UIBlock2D panel;

    [Header("SPEAKER DATABASE")]
    [SerializeField] private SpeakerDatabase speakerDatabase;

    [Header("TYPING")]
    [SerializeField] private float typingSpeed = 0.02f;

    public System.Action OnTypingComplete;

    private Coroutine typewriterRoutine;
    private bool isTyping;
    private string fullText;

    void Start(){
        HideDialogue();
    }

    void Update(){
        if(isTyping && Keyboard.current.spaceKey.wasPressedThisFrame) SkipTyping();
    }

    public void ShowDialogue(SpeakerType speakerType, string text){
        var speaker = speakerDatabase.GetSpeaker(speakerType);
        if(speaker != null){
            speakerNameText.Text = speaker.displayName;
            speakerNameText.Color = speaker.textColor;
            speakerLine.Color = speaker.textColor;
            if(speaker.icon != null){
                speakerIcon.SetImage(speaker.icon);
                speakerIcon.CalculateLayout();
            }
        }

        panel.gameObject.SetActive(true);

        if(typewriterRoutine != null) StopCoroutine(typewriterRoutine);
        typewriterRoutine = StartCoroutine(Typewriter(text));
    }

    public void ShowDialogueWithCustomName(string customName, SpeakerType speakerType, string text){
        var speaker = speakerDatabase.GetSpeaker(speakerType);
        if(speaker != null){
            speakerNameText.Text = customName;
            speakerNameText.Color = speaker.textColor;
            speakerLine.Color = speaker.textColor;
            if(speaker.icon != null){
                speakerIcon.SetImage(speaker.icon);
                speakerIcon.CalculateLayout();
            }
        }

        panel.gameObject.SetActive(true);

        if(typewriterRoutine != null) StopCoroutine(typewriterRoutine);
        typewriterRoutine = StartCoroutine(Typewriter(text));
    }

    IEnumerator Typewriter(string text){
        isTyping = true;
        fullText = text;
        dialogueText.Text = "";
        foreach(char c in text){
            dialogueText.Text += c;
            yield return new WaitForSeconds(typingSpeed);
            while(GameStateManager.Instance.CurrentState != GameState.Playing)
                yield return null;
        }
        isTyping = false;
        OnTypingComplete?.Invoke();
    }

    public void SkipTyping(){
        if(!isTyping) return;
        if(typewriterRoutine != null) StopCoroutine(typewriterRoutine);
        dialogueText.Text = fullText;
        isTyping = false;
        OnTypingComplete?.Invoke();
    }

    public void HideDialogue() => panel.gameObject.SetActive(false);
    public bool IsVisible() => panel.gameObject.activeSelf;
    public bool IsTyping() => isTyping;
}