using UnityEngine;
using Nova;
using NaughtyAttributes;

[System.Serializable]
public class RecipeVisual : ItemVisuals
{
    [Header("REFERENCES")]
    public UIBlock actionBlock;
    public TextBlock actionLabel;
    public TextBlock ingredientName;
    [Space(10)]
    public UIBlock2D leftIcon;
    public UIBlock2D rightIcon;
}