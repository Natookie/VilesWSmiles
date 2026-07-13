using UnityEngine;
using Nova;
using NaughtyAttributes;

public class LocalizedTextBlock : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private TextBlock textBlock;
    [SerializeField] private string localizationKey;

    void Awake(){
        if(textBlock) return;
        AssignTextBlock();
    }

    void Start(){
        Refresh();
        LocalizationManager.Instance.OnLanguageChanged += Refresh;
    }

    void Refresh() => textBlock.Text = LocalizationManager.Instance.GetText(localizationKey);
    void OnDestroy() => LocalizationManager.Instance.OnLanguageChanged -= Refresh;

    [Button("Assign Text Block", EButtonEnableMode.Editor)]
    public void AssignTextBlock() => textBlock ??= GetComponent<TextBlock>();
}