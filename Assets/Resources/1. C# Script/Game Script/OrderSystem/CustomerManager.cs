using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("REFERENCES")]
    [SerializeField] private CustomerLibrary customerLibrary;
    [SerializeField] private CustomerController customerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Chair[] chairs;
    private DialogueLibrary dialogueLibrary;

    [Header("SHIFT SYSTEM")]
    [SerializeField] private ShiftData[] shifts;
    [SerializeField] private Vector2 delayBeforeSpawn;
    
    [Header("SETTINGS")]
    [SerializeField] private int maxCustomers = 4;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(5f, 15f);
    [ReadOnly] public int currentShiftNumber;
    [ReadOnly] public int currentShiftIndex = 0;

    private List<CustomerController> activeCustomers = new List<CustomerController>();
    private List<Chair> availableChairs = new List<Chair>();
    private bool isSpawning = false;
    
    private int customersServedThisShift = 0;
    private int customersSpawnedThisShift = 0;
    private ShiftData CurrentShift => shifts[currentShiftIndex];
    private bool isShiftActive = false;
    private bool hasStartedShift = false;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start(){
        currentShiftIndex = SaveManager.Instance.CurrentData.shiftIndex;
        dialogueLibrary = DialogueLibrary.Instance;
        GameStateManager.Instance.OnStateChanged += OnStateChanged;

        if(SaveManager.Instance.CurrentData.hasFinishedGame) return;
        if(!hasStartedShift && SaveManager.Instance.CurrentData.hasCompletedTutorial){
            hasStartedShift = true;
            StartCoroutine(StartShiftWithSequence());
        }
    }

    public void SetupSceneReferences(Chair[] sceneChairs, Transform sceneSpawnPoint){
        chairs = sceneChairs;
        spawnPoint = sceneSpawnPoint;
        availableChairs.Clear();
        availableChairs.AddRange(chairs);
    }

    void OnStateChanged(GameState previousState, GameState newState){
        if(newState != GameState.Playing) return;
        if(SaveManager.Instance.CurrentData.hasFinishedGame) return;
        
        if(!hasStartedShift && SaveManager.Instance.CurrentData.hasCompletedTutorial){
            hasStartedShift = true;
            StartCoroutine(StartShiftWithSequence());
        }
        else if(isShiftActive && !isSpawning) StartSpawning();
    }

    public void StartFirstShift(){
        currentShiftIndex = 0;
        SaveManager.Instance.CurrentData.shiftIndex = 0;
        SaveManager.Instance.Save();
        StartCoroutine(StartShiftWithSequence());
    }
    IEnumerator StartShiftWithSequence(){
        yield return StartCoroutine(dialogueLibrary.PlayShiftSequence(CurrentShift.shiftNumber));
        StartNextShift();
    }

    public void StartNextShift(){
        if(currentShiftIndex >= shifts.Length){
            Debug.Log("All shifts completed!");
            GameManager.Instance.OnAllShiftsCompleted();
            return;
        }
        
        isShiftActive = true;
        customersServedThisShift = 0;
        customersSpawnedThisShift = 0;
        StartSpawning();
    }

    public void StartSpawning(){
        if(isSpawning) return;
        isSpawning = true;
        
        if(CurrentShift.usePreDeterminedCustomers) StartCoroutine(PreDeterminedSpawnRoutine());
        else StartCoroutine(SpawnRoutine());
    }

    IEnumerator PreDeterminedSpawnRoutine(){
        foreach(var entry in CurrentShift.spawnSequence){
            yield return new WaitForSeconds(entry.delayBeforeSpawn);
            while(activeCustomers.Count >= maxCustomers || availableChairs.Count == 0)
                yield return new WaitForSeconds(0.5f);
            
            SpawnSpecificCustomer(entry.firstName, entry.lastName);
        }
    }

    IEnumerator SpawnRoutine(){
        float delay = Random.Range(delayBeforeSpawn.x, delayBeforeSpawn.y);
        yield return new WaitForSeconds(delay);

        int maxSpawns = CurrentShift.customersToServe + Random.Range(0, 3);
        while(isShiftActive && customersSpawnedThisShift < maxSpawns){
            if(activeCustomers.Count < maxCustomers && availableChairs.Count > 0){
                SpawnCustomer();
                customersSpawnedThisShift++;
            }
            yield return new WaitForSeconds(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
        }
    }

    public void SpawnCustomer(bool useRandom = true){
        CustomerData data = useRandom ? customerLibrary.GenerateRandom() : customerLibrary.GenerateMarco();
        SpawnCustomerWithData(data);
    }

    public void SpawnSpecificCustomer(string firstName, string lastName){
        CustomerData data = customerLibrary.GetCustomerByName(firstName, lastName);
        if(data != null) SpawnCustomerWithData(data);
    }

    void SpawnCustomerWithData(CustomerData data){
        CustomerController customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity, this.transform);
        if(CurrentShift.shiftNumber == 5) customer.SetTimerActive(false);
        customer.Initialize(data);

        Chair chair = availableChairs[0];
        availableChairs.RemoveAt(0);
        chair.Occupy(customer);
        customer.Chair = chair;

        customer.OnLeave += () => OnCustomerLeave(customer);
        customer.OnOrderCompleted += (cust, success) => OnCustomerOrderCompleted(cust, success);
        customer.OnClicked += (c) => OnCustomerClicked?.Invoke(c);

        customer.MoveToChair();
        activeCustomers.Add(customer);
        AudioManager.Instance.PlaySFX(SFXSound.DoorChime);
    }

    void OnCustomerOrderCompleted(CustomerController customer, bool success){
        int moneyEarned = 0;
        string reactionKey;
        
        if(!SaveManager.Instance.CurrentData.hasCompletedTutorial) return;
        if(success){
            int failureCount = DrinkMaker.Instance.GetFailureCount();
            switch(failureCount){
                case 0: 
                    moneyEarned = 15;
                    reactionKey = GetRandomKey("Customer_Reaction_Perfect");
                    break;
                case 1: 
                    moneyEarned = 10;
                    reactionKey = GetRandomKey("Customer_Reaction_Good");
                    break;
                case 2: 
                    moneyEarned = 5;
                    reactionKey = GetRandomKey("Customer_Reaction_Okay");
                    break;
                default: 
                    moneyEarned = 0;
                    reactionKey = GetRandomKey("Customer_Reaction_Fail");
                    break;
            }
            GameManager.Instance.AddMoney(moneyEarned);
        }
        else{
            GameManager.Instance.AddMoney(-10);
            reactionKey = GetRandomKey("Customer_Reaction_Fail");
        }
        
        if(CurrentShift.shiftNumber == 5 && success){
            customersServedThisShift++;
            dialogueLibrary.StartCoroutine(PlayShift5Ending());
            return;
        }
        
        if(CurrentShift.shiftNumber == 5){
            DialogueManager.Instance.PlayCustomerReaction(customer.FullName, reactionKey, customer.Data.orderedRecipe);
            StartCoroutine(PlayShift5Ending());
            return;
        }

        DialogueManager.Instance.PlayCustomerReaction(customer.FullName, reactionKey, customer.Data.orderedRecipe);
        customersServedThisShift++;
        CheckShiftCompletion();
    }

    IEnumerator PlayShift5Ending(){
        yield return StartCoroutine(dialogueLibrary.PlayShift5EndingSequence());
        currentShiftIndex++;
        SaveManager.Instance.CurrentData.shiftIndex = currentShiftIndex;
        SaveManager.Instance.Save();
        isShiftActive = false;
        isSpawning = false;
        GameManager.Instance.OnAllShiftsCompleted();
    }

    string GetRandomKey(string baseKey){
        int variant = Random.Range(1, 4);
        return $"{baseKey}_{variant}";
    }

    void CheckShiftCompletion(){
        if(customersServedThisShift >= CurrentShift.customersToServe)
            CompleteShift();
    }

    void CompleteShift(){
        isShiftActive = false;
        isSpawning = false;
        StopAllCoroutines();
        
        foreach(var customer in activeCustomers) customer.Leave();
        StartCoroutine(WaitForCustomersToLeave());
    }

    IEnumerator WaitForCustomersToLeave(){
        while(activeCustomers.Count > 0) yield return new WaitForSeconds(0.5f);
        currentShiftIndex++;
        SaveManager.Instance.CurrentData.shiftIndex = currentShiftIndex;
        SaveManager.Instance.Save();
        
        yield return new WaitForSeconds(2f);
        if(currentShiftIndex < shifts.Length) StartCoroutine(StartShiftWithSequence());
    }

    void OnCustomerLeave(CustomerController customer){
        if(customer.Chair != null){
            customer.Chair.Vacate();
            availableChairs.Add(customer.Chair);
        }
        activeCustomers.Remove(customer);
        if(DrinkMaker.Instance.GetCurrentCustomer() == customer) DrinkMaker.Instance.FailDrink();
    }

    public System.Action<CustomerController> OnCustomerClicked;
    public Transform GetSpawnPoint() => spawnPoint;
    
    public int GetCurrentShiftNumber() => CurrentShift?.shiftNumber ?? 0;
    public int GetCustomersServedThisShift() => customersServedThisShift;
    public int GetCustomersRemaining() => CurrentShift.customersToServe - customersServedThisShift;
}