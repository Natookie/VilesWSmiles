using UnityEngine;
using NaughtyAttributes;

public enum SliderValueType
{
    Numeric,
    Word
}

[CreateAssetMenu(fileName = "SliderConfig", menuName = "UI/Slider Config")]
public class MySliderConfig : ScriptableObject
{
    public SliderValueType valueType;
    [HideIf("usingWord")] public Vector2 valueConstraint = new Vector2(0f, 100f);
    [HideIf("usingWord")] public bool wholeNumbers = true;
    [HideIf("usingWord")] public string valueFormat = "0";
    [ShowIf("usingWord")] public string[] wordOptions;
    [Space(10)]
    [ReadOnly] public bool UsingWord;
    [ReadOnly] public int MaxCount;
    [ReadOnly] public int StepCount;

    bool usingWord => valueType == SliderValueType.Word;
    public int maxCount => usingWord ? wordOptions.Length : (int)valueConstraint.y;
    public int stepCount => usingWord ? maxCount : 1;

    [Button("Sync", EButtonEnableMode.Editor)]
    public void Sync(){
        UsingWord = usingWord;
        MaxCount = maxCount;
        StepCount = stepCount;
    }

    public string GetValueText(float normalizedValue){
        if(usingWord && wordOptions != null && wordOptions.Length > 0){
            int index = Mathf.RoundToInt(normalizedValue * (wordOptions.Length - 1));
            index = Mathf.Clamp(index, 0, wordOptions.Length - 1);
            return wordOptions[index];
        }

        float value = Mathf.Lerp(valueConstraint.x, valueConstraint.y, normalizedValue);
        if(wholeNumbers) value = Mathf.Round(value);
        return value.ToString(valueFormat);
    }
}