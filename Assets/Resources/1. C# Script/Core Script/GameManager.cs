using UnityEngine;
using System.Collections;
using NaughtyAttributes;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    private const int totalDebt = 50000;
    [Header("MONEY REFERENCES")]
    [ReadOnly] public int currentMoney;
    [SerializeField] private TextMeshProUGUI debtText;

    private int displayedDebt;
    private int remainingDebt;
    private Coroutine scrollRoutine;

    public bool allowInteraction = false;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentMoney = 0;
        remainingDebt = totalDebt;
        displayedDebt = totalDebt;
    }

    void Start(){
        StartCoroutine(SyncMoney());
        UpdateDebtDisplay();
    }

    IEnumerator SyncMoney(){
        yield return new WaitForEndOfFrame();
        currentMoney = SaveManager.Instance.CurrentData.accumulatedMoney;
        remainingDebt = totalDebt - currentMoney;
        if(remainingDebt < 0) remainingDebt = 0;
        displayedDebt = remainingDebt;
        UpdateDebtDisplay();
    }

    public void SetupDebtText(TextMeshProUGUI text){
        debtText = text;
        UpdateDebtDisplay();
    }

    public void AddMoney(int amount){
        currentMoney += amount;
        SaveManager.Instance.CurrentData.accumulatedMoney = currentMoney;

        int newRemainingDebt = totalDebt - currentMoney;
        if(newRemainingDebt < 0) newRemainingDebt = 0;

        if(gameObject.activeInHierarchy && debtText != null){
            if(scrollRoutine != null) StopCoroutine(scrollRoutine);
            scrollRoutine = StartCoroutine(ScrollDebt(newRemainingDebt));
        }
        else{
            remainingDebt = newRemainingDebt;
            displayedDebt = remainingDebt;
            UpdateDebtDisplay();
        }
    }

    [SerializeField] private float scrollDuration = 2f;
        IEnumerator ScrollDebt(int target){
        int startValue = displayedDebt;
        float elapsed = 0f;
        
        while(elapsed < scrollDuration){
            elapsed += Time.deltaTime;
            float t = elapsed / scrollDuration;
            displayedDebt = (int)Mathf.Lerp(startValue, target, t);
            UpdateDebtDisplay();
            yield return null;
        }
        
        displayedDebt = target;
        remainingDebt = target;
        UpdateDebtDisplay();
    }

    void UpdateDebtDisplay(){
        if(debtText != null) debtText.text = $"${displayedDebt:N0}";
    }

    public int GetMoney() => currentMoney;

    public void OnAllShiftsCompleted(){
        SaveManager.Instance.CurrentData.hasFinishedGame = true;
        SaveManager.Instance.Save();
        GameStateManager.Instance.ShowMainMenu();
    }
}