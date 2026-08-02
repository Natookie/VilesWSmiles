using UnityEngine;
using NaughtyAttributes;
using Nova;
using System.Text.RegularExpressions;

public class RecipeHandler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private RecipeAction action;
    [SerializeField] private Ingredient ingredient;
    [Space(10)]
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    [Header("QUICK PREVIEW")]
    [SerializeField] private string actionString;
    [SerializeField] private string ingredientString;

    [ReadOnly] public bool needTool;
    [ReadOnly] public ItemView view;
    [ReadOnly] public RecipeVisual vis;

    [Button("1. Assign ItemView", EButtonEnableMode.Editor)]
    public void AssignView(){
        view = GetComponent<ItemView>();
        vis = view.Visuals as RecipeVisual;
    }

    [Button("2. Assign Reference", EButtonEnableMode.Editor)]
    public void AssignReference(){
        vis.actionBlock    = FindInChildren<UIBlock2D>("Recipe Icon Block");
        vis.actionLabel    = FindInChildren<TextBlock>("Recipe Action");
        vis.ingredientName = FindInChildren<TextBlock>("Recipe Name");
        vis.leftIcon       = FindInChildren<UIBlock2D>("Recipe Icon");
        vis.rightIcon      = FindInChildren<UIBlock2D>("Icon");
    }

    private T FindInChildren<T>(string objectName) where T : Component{
        foreach(Transform child in GetComponentsInChildren<Transform>(true)){
            if(child.name == objectName) return child.GetComponent<T>();
        }
        return null;
    }

    [Button("3. Assign Value", EButtonEnableMode.Editor)]
    public void AssignValue(){
        needTool = CheckToolRequirement();

        vis.actionBlock.Color = !needTool ? new Color32(113, 170, 52, 255) : new Color32(138, 235, 241, 255);
        vis.actionLabel.Text = action.ToString();

        if(ingredient == Ingredient.None){
            switch(action){
                case RecipeAction.Stir:
                    vis.ingredientName.Text = $"Using Bar Spoon";
                    break;
                case RecipeAction.Shake:
                    vis.ingredientName.Text = $"Using Shaker Tin";
                    break;
            }
        }
        else{
            bool useWith = action == RecipeAction.Top || action == RecipeAction.Garnish;
            vis.ingredientName.Text = $"{(useWith ? "With " : "")}{FormatEnumName(ingredient)}";
        }

        vis.leftIcon.SetImage(leftSprite);
        vis.rightIcon.SetImage(rightSprite);

        ingredientString = vis.ingredientName.Text;
        actionString = vis.actionLabel.Text;
    }

    bool CheckToolRequirement(){
        switch(action){
            case RecipeAction.Stir:
            case RecipeAction.Shake:
            case RecipeAction.Cut:
            case RecipeAction.Juice:
                return true;
        }
        return false;
    }
    string FormatEnumName(System.Enum value) => Regex.Replace(value.ToString(), "(\\B[A-Z])", " $1");
}