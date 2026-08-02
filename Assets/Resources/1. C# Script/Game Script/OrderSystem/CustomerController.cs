using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float arrivalThreshold = 0.1f;

    [Header("POSITIONING")]
    [SerializeField] private float walkingY = -1f;
    [SerializeField] private float sittingY = -1.38f;
    [SerializeField] private bool manualTweak;

    [Header("TIMER")]
    [SerializeField] private Canvas timerCanvas;
    [SerializeField] private Image timerFillBar;
    [SerializeField] private float patienceTime = 30f;
    [SerializeField] private bool useTimer = true;

    [Header("ANIMATION")]
    [SerializeField] private Sprite[] walkingCycle;
    [SerializeField] private Sprite sittingPose;
    [SerializeField] private float walkFrameRate = 0.15f;
    private SpriteRenderer sr;
    private int walkIndex;
    private float walkTimer;

    public CustomerData Data { get; private set; }
    public string FullName => $"{Data.firstName} {Data.lastName}";
    public CustomerCategory ToleranceCategory { get; private set; }
    public CustomerCategory MoneyCategory { get; private set; }
    public float tolerance => Data.alcoholTolerance;
    public float money => Data.money;

    public Chair Chair { get; set; }
    public bool HasReachedChair { get; private set; }
    public bool HasBeenServed { get; private set; }

    public System.Action OnLeave;
    public System.Action<CustomerController> OnClicked;
    public System.Action<CustomerController, bool> OnOrderCompleted;

    private Rigidbody2D rb;
    private bool isMoving;
    private bool isLeaving = false;
    private Transform spawnPoint;
    private Vector2? exitTarget;
    private float currentPatience;
    private bool timerActive;
    private Coroutine deliverCoroutine;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if(CustomerManager.Instance != null) spawnPoint = CustomerManager.Instance.GetSpawnPoint();
    }

    void Start(){
        timerCanvas.worldCamera = Camera.main;
    }

    public void Initialize(CustomerData data){
        Data = data;
        ToleranceCategory = GetCategory(data.alcoholTolerance, 30f, 60f);
        MoneyCategory = GetCategory(data.money, 300, 600);
        
        if(useTimer){
            currentPatience = patienceTime;
            timerActive = true;
            timerCanvas.gameObject.SetActive(true);
            timerFillBar.fillAmount = 1f;
        }
        else{
            timerCanvas.gameObject.SetActive(false);
            timerActive = false;
        }
    }

    void Update(){
        if(isMoving && walkingCycle.Length > 0){
            walkTimer += Time.deltaTime;
            if(walkTimer >= walkFrameRate){
                walkTimer = 0f;
                walkIndex = (walkIndex + 1) % walkingCycle.Length;
                sr.sprite = walkingCycle[walkIndex];
            }
        }
        
        if(manualTweak && HasReachedChair && !isMoving){
            Vector2 pos = transform.position;
            pos.y = sittingY;
            transform.position = pos;
        }
        
        if(!timerActive || !HasReachedChair) return;
        
        currentPatience -= Time.deltaTime;
        timerFillBar.fillAmount = currentPatience / patienceTime;
        
        if(currentPatience <= 0){
            timerActive = false;
            timerCanvas.gameObject.SetActive(false);
            Leave();
        }
    }

    public void MoveToChair(){
        if(Chair == null) return;
        rb.gravityScale = 0f;
        isMoving = true;
    }

    public void ServeDrink(bool success){
        if(HasBeenServed || isLeaving) return;
        
        timerActive = false;
        timerCanvas.gameObject.SetActive(false);
        HasBeenServed = true;
        OnOrderCompleted?.Invoke(this, success);
        
        StartCoroutine(LeaveAfterDialogue());
    }

    IEnumerator LeaveAfterDialogue(){
        while(DialogueManager.Instance.IsDialogueVisible())
            yield return null;
        yield return new WaitForSeconds(0.5f);
        Leave();
    }

    public void Leave(){
        if(isLeaving) return;
        timerActive = false;
        timerCanvas.gameObject.SetActive(false);
        rb.gravityScale = 0f;
        isMoving = true;
        isLeaving = true;
        exitTarget = spawnPoint != null ? (Vector2)spawnPoint.position : (Vector2)Vector2.zero;
        sr.flipX = true;
    }

    void FixedUpdate(){
        if(!isMoving) return;

        Vector2 targetPos;
        if(exitTarget.HasValue) targetPos = exitTarget.Value;
        else if(Chair != null) targetPos = (Vector2)Chair.transform.position;
        else{
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            return;
        }

        Vector2 currentPos = transform.position;
        float horizontalDistance = targetPos.x - currentPos.x;

        if(Mathf.Abs(horizontalDistance) > arrivalThreshold){
            float moveX = Mathf.Sign(horizontalDistance) * moveSpeed;
            rb.linearVelocity = new Vector2(moveX, 0f);
            Vector2 pos = transform.position;
            pos.y = walkingY;
            transform.position = pos;
        }
        else{
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            isMoving = false;
            
            Vector2 pos = transform.position;
            
            if(exitTarget.HasValue){
                transform.position = pos;
                exitTarget = null;
                OnLeave?.Invoke();
                Destroy(gameObject);
            }
            else{
                pos.y = sittingY;
                transform.position = pos;
                HasReachedChair = true;
                sr.sprite = sittingPose;
            }
        }
    }

    void OnMouseDown(){
        if(!HasReachedChair || HasBeenServed || isLeaving) return;
        if(!GameManager.Instance.allowInteraction) return;

        if(DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueTyping()){
            Debug.Log("Cannot interact while dialogue is typing");
            return;
        }
        
        InteractWithCustomer();
        DialogueManager.Instance.TriggerInteraction("CustomerClick");
    }

    public void InteractWithCustomer(){
        if(DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueTyping()){
            Debug.Log("Cannot interact while dialogue is typing");
            return;
        }
        
        if(DrinkMaker.Instance != null && DrinkMaker.Instance.IsDrinkReady() && DrinkMaker.Instance.GetCurrentCustomer() == this){
            if(deliverCoroutine != null) StopCoroutine(deliverCoroutine);
            deliverCoroutine = StartCoroutine(DeliverDrinkWhenReady());
        }
        else if(DrinkMaker.Instance == null || !DrinkMaker.Instance.IsDrinkReady()){
            OnClicked?.Invoke(this);
        }
    }

    IEnumerator DeliverDrinkWhenReady(){
        while(DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueTyping()){
            yield return null;
        }
        yield return null;
        
        if(DrinkMaker.Instance != null && DrinkMaker.Instance.IsDrinkReady() && DrinkMaker.Instance.GetCurrentCustomer() == this){
            if(GameManager.Instance.allowInteraction) DrinkMaker.Instance.DeliverToCustomer();
        }
        
        deliverCoroutine = null;
    }

    public void SetTimerActive(bool value) => useTimer = value;

    CustomerCategory GetCategory(float value, float lowMax, float medMax){
        if(value <= lowMax) return CustomerCategory.Low;
        if(value <= medMax) return CustomerCategory.Medium;
        return CustomerCategory.High;
    }
}