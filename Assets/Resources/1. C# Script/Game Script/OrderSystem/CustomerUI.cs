using UnityEngine;
using System.Collections;
using Nova;
using NaughtyAttributes;

public class CustomerUI : MonoBehaviour
{
    [Header("PANEL")]
    [SerializeField] private SlidingPanel panel;

    [Header("TOP INFO")]
    [SerializeField] private TextBlock customerName;
    [SerializeField] private TextBlock customerOrder;
    [SerializeField] private TextBlock orderDetail;

    [Header("LOW INFO")]
    [SerializeField] private TextBlock tolerance;
    [SerializeField] private TextBlock money;
    [Space(5)]
    [SerializeField] private UIBlock2D toleranceBlock;
    [SerializeField] private UIBlock2D moneyBlock;
    [Space(10)]
    [SerializeField] private Color[] displayColor = new Color[3];

    [Header("DRINK INFO")]
    [SerializeField] private TextBlock drinkInfoText;
    
    [ReadOnly] public CustomerController currentCustomer;
    private string currentStep = "0/0";
    private string selectedItem = "None";
    private string failCount = "0/3";

    void Start(){
        CustomerManager.Instance.OnCustomerClicked += ShowCustomerInfo;
        DrinkMaker.Instance.OnProgressUpdated += UpdateStepProgress;
        DrinkMaker.Instance.OnIngredientSelected += UpdateSelectedItem;
        DrinkMaker.Instance.OnToolSelected += UpdateSelectedTool;
        DrinkMaker.Instance.OnSelectionCleared += ClearSelectedItem;
        DrinkMaker.Instance.OnFailureCountUpdated += UpdateFailCount;
        DrinkMaker.Instance.OnDrinkServed += HideCustomerInfo;
    }

    void UpdateStepProgress(int stepIndex, int repeatCount){
        Recipe recipe = DrinkMaker.Instance.GetCurrentRecipe();
        if(recipe != null)
            currentStep = $"{stepIndex + 1}/{recipe.steps.Length}";
        else
            currentStep = "0/0";
        UpdateDrinkInfo();
    }

    void UpdateSelectedItem(Ingredient ingredient){
        selectedItem = ingredient.ToString();
        UpdateDrinkInfo();
    }

    void UpdateSelectedTool(Tool tool){
        selectedItem = tool.ToString();
        UpdateDrinkInfo();
    }

    void ClearSelectedItem(){
        selectedItem = "None";
        UpdateDrinkInfo();
    }

    void UpdateFailCount(int count){
        failCount = $"{count}/3";
        UpdateDrinkInfo();
    }

    void UpdateDrinkInfo(){
        if(drinkInfoText != null)
            drinkInfoText.Text = $"Current Step\t: {currentStep}\nSelected\t: {selectedItem}\nFail Count\t: {failCount}";
    }

    void HideCustomerInfo(CustomerController customer, bool success){
        panel.HidePanel();
        currentCustomer = null;
        currentStep = "0/0";
        selectedItem = "None";
        failCount = "0/3";
    }

    void ShowCustomerInfo(CustomerController customer){
        if(currentCustomer != null) return;
        currentCustomer = customer;
        panel.ShowPanel();

        customerName.Text = customer.FullName;
        tolerance.Text = $"<color=#5a5353>Alcohol Tolerance\t:</color> {customer.tolerance}%";
        money.Text = $"<color=#5a5353>Predicted Wealth\t:</color> ${customer.money}";
        customerOrder.Text = customer.Data.orderedRecipe;
        
        Recipe recipe = RecipeBook.GetRecipe(customer.Data.orderedRecipe);
        int totalSteps = recipe != null ? recipe.steps.Length : 0;
        currentStep = $"0/{totalSteps}";
        selectedItem = "None";
        failCount = "0/3";
        UpdateDrinkInfo();
        
        Color t_color = Color.black, m_color = Color.black;
        switch(customer.ToleranceCategory){
            case CustomerCategory.Low: t_color = displayColor[0]; break;
            case CustomerCategory.Medium: t_color = displayColor[1]; break;
            case CustomerCategory.High: t_color = displayColor[2]; break;
        }
        switch(customer.MoneyCategory){
            case CustomerCategory.Low: m_color = displayColor[0]; break;
            case CustomerCategory.Medium: m_color = displayColor[1]; break;
            case CustomerCategory.High: m_color = displayColor[2]; break;
        }

        toleranceBlock.Border.Color = t_color;
        moneyBlock.Border.Color = m_color;
        
        AudioManager.Instance.PlayUI(UISound.CustomerOrdered);
    }

    void OnDestroy(){
        if(CustomerManager.Instance != null) CustomerManager.Instance.OnCustomerClicked -= ShowCustomerInfo;
        if(DrinkMaker.Instance != null){
            DrinkMaker.Instance.OnProgressUpdated -= UpdateStepProgress;
            DrinkMaker.Instance.OnIngredientSelected -= UpdateSelectedItem;
            DrinkMaker.Instance.OnToolSelected -= UpdateSelectedTool;
            DrinkMaker.Instance.OnSelectionCleared -= ClearSelectedItem;
            DrinkMaker.Instance.OnFailureCountUpdated -= UpdateFailCount;
            DrinkMaker.Instance.OnDrinkServed -= HideCustomerInfo;
        }
    }
}