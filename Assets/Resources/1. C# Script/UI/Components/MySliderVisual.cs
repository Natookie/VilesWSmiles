using UnityEngine;
using Nova;
using NaughtyAttributes;

[System.Serializable]
public class MySliderVisual : ItemVisuals
{
    [Header("REFERENCES")]
    public UIBlock2D icon;
    public TextBlock label;
    public UIBlock2D background;
    public UIBlock2D fill;
    [Space(5)]
    public UIBlock2D draggable;
    public TextBlock data;

    [Header("COLOR ATTRIBUTE")]
    public Color32 background_color;
    public Color32 fill_color;
    [Space(10)]
    public Color32 draggable_unhoverColor;
    public Color32 draggable_hoverColor;
    public Color32 draggable_clickedColor;
    [Space(10)]
    public Color32 data_unhoverColor;
    public Color32 data_hoverColor;
    public Color32 data_clickedColor;

    [Header("TRANSFORM ATTRIBUTE")]
    [MinValue(1f)] public float clickScale = 1f;
    [MinValue(1f)] public float xStretchScale = 1f;

    [Header("COLOR TWEAK")]
    [MinValue(0f)] public float colorDuration = 0.2f;
    public AnimationCurve colorCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
}