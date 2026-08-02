using UnityEngine;
using System.Collections.Generic;

public enum CustomerCategory
{
    Low,
    Medium,
    High
}

public enum Gender
{
    Male,
    Female
}

[System.Serializable]
public class CustomerData
{
    public string firstName;
    public string lastName;
    public Gender gender;
    public Sprite appearance;
    public float alcoholTolerance;
    public int money;
    public string orderedRecipe;
}

[CreateAssetMenu(fileName = "CustomerLibrary", menuName = "Customer/Customer Library")]
public class CustomerLibrary : ScriptableObject
{
    [Header("FIRST NAMES")]
    [SerializeField] private string[] maleFirstNames;
    [SerializeField] private string[] femaleFirstNames;

    [Header("LAST NAMES")]
    [SerializeField] private string[] lastNames;

    [Header("APPEARANCES")]
    [SerializeField] private Sprite[] maleSprites;
    [SerializeField] private Sprite[] femaleSprites;

    [Header("STATS RANGE")]
    [SerializeField] private Vector2 lowAlcoholRange = new Vector2(0f, 30f);
    [SerializeField] private Vector2 medAlcoholRange = new Vector2(31f, 60f);
    [SerializeField] private Vector2 highAlcoholRange = new Vector2(61f, 100f);
    [Space(10)]
    [SerializeField] private Vector2 lowMoneyRange = new Vector2(0, 300);
    [SerializeField] private Vector2 medMoneyRange = new Vector2(301, 600);
    [SerializeField] private Vector2 highMoneyRange = new Vector2(601, 1000);

    [Header("RECIPES")]
    [SerializeField] private string[] recipePool;

    [Header("PREDEFINED CUSTOMERS")]
    [SerializeField] private List<CustomerData> predefinedCustomers = new List<CustomerData>();

    public CustomerData GenerateRandom(){
        CustomerData data = new CustomerData();
        data.gender = Random.value > 0.5f ? Gender.Male : Gender.Female;
        data.firstName = GetRandomFirstName(data.gender);
        data.lastName = lastNames[Random.Range(0, lastNames.Length)];
        data.appearance = GetRandomAppearance(data.gender);

        CustomerCategory toleranceCat = GetRandomCategory();
        CustomerCategory moneyCat = GetRandomCategory();

        data.alcoholTolerance = Mathf.Round(GetRandomInRange(toleranceCat, lowAlcoholRange, medAlcoholRange, highAlcoholRange));
        data.money = Mathf.RoundToInt(GetRandomInRange(moneyCat, lowMoneyRange, medMoneyRange, highMoneyRange));
        data.orderedRecipe = recipePool[Random.Range(0, recipePool.Length)];

        return data;
    }

    public CustomerData GenerateMarco(){
        CustomerData data = new CustomerData();
        data.gender = Gender.Male;
        data.firstName = "Marco";
        data.lastName = "Dimarlo";
        data.appearance = GetRandomAppearance(data.gender);

        CustomerCategory toleranceCat = CustomerCategory.Low;
        CustomerCategory moneyCat = CustomerCategory.Medium;

        data.alcoholTolerance = Mathf.Round(GetRandomInRange(toleranceCat, lowAlcoholRange, medAlcoholRange, highAlcoholRange));
        data.money = Mathf.RoundToInt(GetRandomInRange(moneyCat, lowMoneyRange, medMoneyRange, highMoneyRange));
        data.orderedRecipe = recipePool[0];

        return data;
    }

    string GetRandomFirstName(Gender gender){
        string[] pool = gender == Gender.Male ? maleFirstNames : femaleFirstNames;
        return pool[Random.Range(0, pool.Length)];
    }

    Sprite GetRandomAppearance(Gender gender){
        Sprite[] pool = gender == Gender.Male ? maleSprites : femaleSprites;
        return pool[Random.Range(0, pool.Length)];
    }

    CustomerCategory GetRandomCategory(){
        float roll = Random.value;
        if(roll < 0.4f) return CustomerCategory.Low;
        if(roll < 0.8f) return CustomerCategory.Medium;
        return CustomerCategory.High;
    }

    public CustomerData GetCustomerByName(string firstName, string lastName){
        return predefinedCustomers.Find(c => c.firstName == firstName && c.lastName == lastName);
    }

    float GetRandomInRange(CustomerCategory cat, Vector2 low, Vector2 med, Vector2 high){
        switch(cat){
            case CustomerCategory.Low: return Random.Range(low.x, low.y);
            case CustomerCategory.Medium: return Random.Range(med.x, med.y);
            case CustomerCategory.High: return Random.Range(high.x, high.y);
        }
        return 0f;
    }
}