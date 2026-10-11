using UnityEngine;

public enum RecipeAction
{
    Add,
    Pour,
    Top,
    Stir,
    Shake,
    Cut,
    Juice,
    Garnish
}

public enum Ingredient
{
    None,
    SugarCube,
    Bitters,
    Whiskey,
    Vodka,
    Rum,
    Gin,
    LimeJuice, //Prep
    LemonJuice, //Prep
    OrangeJuice, //Prep
    Cola,
    SodaWater,
    Tonic,
    Orange,
    OrangePeel, //Prep
    Lime,
    LimeWedge, //Prep
    Lemon,
    LemonTwist, //Prep
    ChunkyIce,
    Ice //Prep
}

public enum Tool
{
    None,
    ShakerTin,
    BarSpoon,
    CuttingBoard,
    Glass,
    Juicer,
    TrashBin
}

public enum ClickableType
{
    Ingredient,
    Tool,
}

public class Recipe
{
    public string recipeName;
    public RecipeStep[] steps;

    public Recipe(string name, RecipeStep[] steps){
        recipeName = name;
        this.steps = steps;
    }
}

public class RecipeStep
{
    public RecipeAction action;
    public Ingredient ingredient;
    public Tool requiredTool;
    public int repeatCount;

    public RecipeStep(RecipeAction action, Ingredient ingredient = Ingredient.None, Tool requiredTool = Tool.None, int repeatCount = 1){
        this.action = action;
        this.ingredient = ingredient;
        this.requiredTool = requiredTool;
        this.repeatCount = repeatCount;
    }
}

public static class RecipeBook
{
    public static Recipe[] AllRecipes = new Recipe[]{
        new Recipe("The Old Coin", new RecipeStep[]{
            new RecipeStep(RecipeAction.Add, Ingredient.SugarCube),
            new RecipeStep(RecipeAction.Add, Ingredient.Bitters, repeatCount: 3),
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Whiskey),
            new RecipeStep(RecipeAction.Stir, requiredTool: Tool.BarSpoon),
            new RecipeStep(RecipeAction.Cut, Ingredient.Orange, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.OrangePeel),
        }),
        new Recipe("Clear Lie", new RecipeStep[]{
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Vodka),
            new RecipeStep(RecipeAction.Juice, Ingredient.Lime, Tool.Juicer), // New! Juice fresh lime
            new RecipeStep(RecipeAction.Add, Ingredient.LimeJuice),
            new RecipeStep(RecipeAction.Shake, requiredTool: Tool.ShakerTin),
            new RecipeStep(RecipeAction.Top, Ingredient.SodaWater),
            new RecipeStep(RecipeAction.Cut, Ingredient.Lime, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.LimeWedge),
        }),
        new Recipe("Dark Favour", new RecipeStep[]{
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Rum),
            new RecipeStep(RecipeAction.Juice, Ingredient.Lime, Tool.Juicer),
            new RecipeStep(RecipeAction.Add, Ingredient.LimeJuice),
            new RecipeStep(RecipeAction.Shake, requiredTool: Tool.ShakerTin),
            new RecipeStep(RecipeAction.Top, Ingredient.Cola),
            new RecipeStep(RecipeAction.Cut, Ingredient.Lime, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.LimeWedge),
        }),
        new Recipe("The Manager's", new RecipeStep[]{
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Gin),
            new RecipeStep(RecipeAction.Juice, Ingredient.Lemon, Tool.Juicer),
            new RecipeStep(RecipeAction.Add, Ingredient.LemonJuice),
            new RecipeStep(RecipeAction.Shake, requiredTool: Tool.ShakerTin),
            new RecipeStep(RecipeAction.Top, Ingredient.Tonic),
            new RecipeStep(RecipeAction.Cut, Ingredient.Lemon, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.LemonTwist),
        }),
        new Recipe("Spiced Heritage", new RecipeStep[]{
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Rum),
            new RecipeStep(RecipeAction.Add, Ingredient.Bitters),
            new RecipeStep(RecipeAction.Add, Ingredient.SugarCube),
            new RecipeStep(RecipeAction.Stir, requiredTool: Tool.BarSpoon),
            new RecipeStep(RecipeAction.Cut, Ingredient.Orange, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.OrangePeel),
        }),
        new Recipe("Morning Reviver", new RecipeStep[]{
            new RecipeStep(RecipeAction.Cut, Ingredient.ChunkyIce, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Add, Ingredient.Ice),
            new RecipeStep(RecipeAction.Pour, Ingredient.Vodka),
            new RecipeStep(RecipeAction.Pour, Ingredient.Gin),
            new RecipeStep(RecipeAction.Juice, Ingredient.Lemon, Tool.Juicer),
            new RecipeStep(RecipeAction.Add, Ingredient.LemonJuice),
            new RecipeStep(RecipeAction.Shake, requiredTool: Tool.ShakerTin),
            new RecipeStep(RecipeAction.Top, Ingredient.SodaWater),
            new RecipeStep(RecipeAction.Cut, Ingredient.Lemon, Tool.CuttingBoard),
            new RecipeStep(RecipeAction.Garnish, Ingredient.LemonTwist),
        }),
    };

    public static Recipe GetRecipe(string name){
        foreach(var recipe in AllRecipes) if(recipe.recipeName == name) return recipe;
        return null;
    }

    public static string[] GetRecipeNames(){
        string[] names = new string[AllRecipes.Length];
        for(int i = 0; i < AllRecipes.Length; i++) names[i] = AllRecipes[i].recipeName;
        return names;
    }
}