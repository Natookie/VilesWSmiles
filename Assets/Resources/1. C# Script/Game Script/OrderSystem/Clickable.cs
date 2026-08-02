using UnityEngine;
using NaughtyAttributes;

public class Clickable : MonoBehaviour
{
    [SerializeField] private ClickableType clickableType;
    [ShowIf("isTool")][SerializeField] private Tool tool;
    [ShowIf("isIngre")][SerializeField] private Ingredient ingredient;

    private Collider2D col;
    private bool isTool => clickableType == ClickableType.Tool;
    private bool isIngre => clickableType == ClickableType.Ingredient;
    private bool isHovered = false;

    void Awake(){
        col = GetComponent<Collider2D>();
    }

    public ClickableType GetClickableType() => clickableType;
    public Tool GetTool() => tool;
    public Ingredient GetIngredient() => ingredient;

    public bool IsInRange(){
        if(PlayerController.Instance == null) return false;
        float distance = Mathf.Abs(PlayerController.Instance.transform.position.x - transform.position.x);
        return distance <= RecipeUI.Instance.GetInteractionRadius();
    }

    void OnMouseDown(){
        if(DrinkMaker.Instance == null) return;
        if(!GameManager.Instance.allowInteraction) return;

        if(!IsInRange()){
            AudioManager.Instance.PlayUI(UISound.ObjectFailed);
            return;
        }

        switch(clickableType){
            case ClickableType.Ingredient:
                if(ingredient == Ingredient.None){
                    Debug.LogWarning($"Clickable {gameObject.name} has Ingredient.None! Please assign an ingredient or change to Tool type.");
                    return;
                }
                DrinkMaker.Instance.SelectIngredient(ingredient);
                RecipeUI.Instance.ShowPanel(this, true);
                break;
            case ClickableType.Tool:
                if(tool == Tool.None){
                    Debug.LogWarning($"Clickable {gameObject.name} has Tool.None! Please assign a tool.");
                    return;
                }
                if(tool == Tool.CuttingBoard || tool == Tool.Glass || tool == Tool.TrashBin || tool == Tool.Juicer) HandleToolInteraction();
                else{
                    DrinkMaker.Instance.SelectTool(tool);
                    RecipeUI.Instance.ShowPanel(this, true);
                }
                break;
        }
    }

    void HandleToolInteraction(){
        switch(tool){
            case Tool.CuttingBoard:
                DrinkMaker.Instance.InteractWithCuttingBoard();
                break;
            case Tool.Glass:
                DrinkMaker.Instance.InteractWithGlass();
                break;
            case Tool.TrashBin:
                DrinkMaker.Instance.InteractWithTrashBin();
                break;
            case Tool.Juicer:
                DrinkMaker.Instance.InteractWithJuicer();
                break;
        }
    }

    void OnMouseOver(){
        if(RecipeUI.Instance == null) return;

        bool isIngredientSelected = DrinkMaker.Instance.selectedIngredient != Ingredient.None;
        bool isToolSelected = DrinkMaker.Instance.selectedTool != Tool.None;

        if(!isIngredientSelected && !isToolSelected){
            if(!isHovered) AudioManager.Instance.PlayUI(UISound.ObjectHovered);
            RecipeUI.Instance.ShowPanel(this, false);
        }
        else if(isIngredientSelected && clickableType == ClickableType.Ingredient){
            if(!isHovered) AudioManager.Instance.PlayUI(UISound.ObjectHovered);
            if(ingredient != Ingredient.None) RecipeUI.Instance.ShowPanel(this, false);
        }
        else if(isIngredientSelected && clickableType == ClickableType.Tool){
            if(!isHovered) AudioManager.Instance.PlayUI(UISound.ObjectHovered);
            if(tool != Tool.None) RecipeUI.Instance.ShowPanel(this, false);
        }
        else if(isToolSelected && clickableType == ClickableType.Tool){
            if(!isHovered) AudioManager.Instance.PlayUI(UISound.ObjectHovered);
            if(tool != Tool.None) RecipeUI.Instance.ShowPanel(this, false);
        }
        else if(isToolSelected && clickableType == ClickableType.Ingredient){
            if(!isHovered) AudioManager.Instance.PlayUI(UISound.ObjectHovered);
            if(ingredient != Ingredient.None) RecipeUI.Instance.ShowPanel(this, false);
        }

        isHovered = true;
    }

    void OnMouseExit(){
        bool isIngredientSelected = DrinkMaker.Instance.selectedIngredient != Ingredient.None;
        bool isToolSelected = DrinkMaker.Instance.selectedTool != Tool.None;

        if(!isIngredientSelected && !isToolSelected) RecipeUI.Instance.HidePanel();
        else if(isIngredientSelected && clickableType == ClickableType.Tool) RecipeUI.Instance.ReturnToLocked();
        else if(isToolSelected && clickableType == ClickableType.Ingredient) RecipeUI.Instance.ReturnToLocked();

        isHovered = false;
    }
}