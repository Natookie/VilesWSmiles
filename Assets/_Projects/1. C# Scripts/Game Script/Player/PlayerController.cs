using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {get; private set;}

    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;

    private bool isFacingRight = true;

    void Awake(){
        if(Instance == null) Instance = this;
        else{
            Destroy(gameObject);
            return;
        }
        rb ??= GetComponent<Rigidbody2D>();
        sr ??= GetComponent<SpriteRenderer>();
    }

    void FixedUpdate(){
        float moveInput = InputManager.Instance.MoveInput;
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if(Mathf.Abs(moveInput) > 0.01f){
            isFacingRight = moveInput > 0f;
            sr.flipX = !isFacingRight;
        }
    }

    public bool IsMoving() => Mathf.Abs(rb.linearVelocity.x) > 0.1f;
    public bool IsFacingRight() => isFacingRight;
}