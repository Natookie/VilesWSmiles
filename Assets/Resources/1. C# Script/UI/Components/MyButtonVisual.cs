using UnityEngine;
using Nova;
using NaughtyAttributes;

[System.Serializable]
public class MyButtonVisual : ItemVisuals
{
    [Header("REFERENCES")]
    public UIBlock2D container;
    public UIBlock2D content;
    public UIBlock2D icon;
    public TextBlock label;

    [Header("COLOR ATTRIBUTE")]
    public Color32 content_unhoverColor;
    public Color32 content_hoverColor;
    public Color32 content_clickedColor;
    [Space(10)]
    public Color32 label_unhoverColor;
    public Color32 label_hoverColor;
    public Color32 label_clickedColor;
    [Space(10)]
    public Color32 icon_unhoverColor;
    public Color32 icon_hoverColor;
    public Color32 icon_clickedColor;

    [Header("TRANSFORM ATTRIBUTE")]
    [MinValue(1f)] public float clickScale = 1f;
    [MinValue(0f)] public Vector2 hoverVector = new Vector2(0f, 0f);

    [Header("SCALE TWEAK")]
    [MinValue(0f)] public float scaleDuration = 0.2f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("HOVER TWEAK")]
    [MinValue(0f)] public float hoverDuration = 0.3f;
    public AnimationCurve hoverCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("COLOR TWEAK")]
    [MinValue(0f)] public float colorDuration = 0.2f;
    public AnimationCurve colorCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
}