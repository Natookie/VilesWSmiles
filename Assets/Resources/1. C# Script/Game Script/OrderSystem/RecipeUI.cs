using UnityEngine;
using Nova;
using System.Text.RegularExpressions;

public class RecipeUI : MonoBehaviour
{
    public static RecipeUI Instance { get; private set; }

    [Header("REFERENCES")]
    [SerializeField] private SlidingPanel panel;
    [SerializeField] private CustomerUI cui;

    [Header("UI REFERENCES")]
    [SerializeField] private UIBlock2D objectBlock;
    [SerializeField] private UIBlock2D objectIcon;
    [SerializeField] private TextBlock objectName;

    [Header("TWEAKS")]
    [SerializeField] private Sprite[] iconSprite;
    [SerializeField] private Color[] blockColor;
    [Space(10)]
    [SerializeField] private Color availableColor;
    [SerializeField] private Color unavailableColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private float interactionRadius = 3f;

    private Transform player;
    private Transform currentTarget;
    private Clickable lockedClickable;
    private Ingredient lockedIngredient;
    private Tool lockedTool;
    private bool isLocked = false;
    private bool isHovering = false;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start(){
        HidePanel();
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update(){
        if(cui.currentCustomer == null){
            objectBlock.Border.Color = unavailableColor;
            return;
        }

        if(isLocked && !isHovering){
            objectBlock.Border.Color = lockedColor;
            return;
        }

        if(currentTarget == null){
            objectBlock.Border.Color = unavailableColor;
            return;
        }

        float distance = Mathf.Abs(player.position.x - currentTarget.position.x);
        bool isInRange = distance <= interactionRadius;
        objectBlock.Border.Color = isInRange ? availableColor : unavailableColor;
    }

    public void ShowPanel(Clickable clickable, bool lockPanel = false){
        if(clickable == null){
            HidePanel();
            return;
        }

        if(lockPanel){
            lockedClickable = clickable;
            lockedIngredient = Ingredient.None;
            lockedTool = Tool.None;
            isLocked = true;
            isHovering = false;
        }
        else{
            isHovering = true;
        }

        currentTarget = clickable.transform;
        panel.ShowPanel();

        switch(clickable.GetClickableType()){
            case ClickableType.Ingredient:
                objectIcon.SetImage(iconSprite[1]);
                objectIcon.Color = blockColor[1];
                objectName.Color = blockColor[1];
                objectName.Text = FormatEnumName(clickable.GetIngredient());
                break;
            case ClickableType.Tool:
                objectIcon.SetImage(iconSprite[0]);
                objectIcon.Color = blockColor[0];
                objectName.Color = blockColor[0];
                objectName.Text = FormatEnumName(clickable.GetTool());
                break;
        }
    }

    public void ShowPanel(Ingredient ingredient, bool lockPanel = false){
        if(ingredient == Ingredient.None){
            HidePanel();
            return;
        }

        if(lockPanel){
            lockedClickable = null;
            lockedIngredient = ingredient;
            lockedTool = Tool.None;
            isLocked = true;
            isHovering = false;
        }
        else{
            isHovering = true;
        }

        currentTarget = null;
        panel.ShowPanel();
        objectIcon.SetImage(iconSprite[1]);
        objectIcon.Color = blockColor[1];
        objectName.Color = blockColor[1];
        objectName.Text = FormatEnumName(ingredient);
    }

    public void ShowPanel(Tool tool, bool lockPanel = false){
        if(tool == Tool.None){
            HidePanel();
            return;
        }

        if(lockPanel){
            lockedClickable = null;
            lockedIngredient = Ingredient.None;
            lockedTool = tool;
            isLocked = true;
            isHovering = false;
        }
        else{
            isHovering = true;
        }

        currentTarget = null;
        panel.ShowPanel();
        objectIcon.SetImage(iconSprite[0]);
        objectIcon.Color = blockColor[0];
        objectName.Color = blockColor[0];
        objectName.Text = FormatEnumName(tool);
    }

    public void ReturnToLocked(){
        if(isLocked){
            isHovering = false;
            if(lockedClickable != null){
                ShowPanel(lockedClickable);
            }
            else if(lockedIngredient != Ingredient.None){
                ShowPanel(lockedIngredient);
            }
            else if(lockedTool != Tool.None){
                ShowPanel(lockedTool);
            }
            else{
                HidePanel();
            }
        }
        else{
            HidePanel();
        }
    }

    public void ClearLock(){
        lockedClickable = null;
        lockedIngredient = Ingredient.None;
        lockedTool = Tool.None;
        isLocked = false;
        isHovering = false;
        HidePanel();
    }

    public void HidePanel(){
        panel.HidePanel();
        currentTarget = null;
        if(!isLocked){
            lockedClickable = null;
            lockedIngredient = Ingredient.None;
            lockedTool = Tool.None;
        }
        isHovering = false;
    }

    public bool IsLocked() => isLocked;
    public float GetInteractionRadius() => interactionRadius;

    string FormatEnumName(System.Enum value) => Regex.Replace(value.ToString(), "(\\B[A-Z])", " $1");
}