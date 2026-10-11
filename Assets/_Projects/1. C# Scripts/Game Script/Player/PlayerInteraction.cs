using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class PlayerInteraction : MonoBehaviour
{
    [Header("INTERACTION")]
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [ReadOnly] public GameObject targetPreview;
    private IInteractable currentTarget;

    private Collider2D[] hitColliders = new Collider2D[10];
    private Transform playerTransform;
    private ContactFilter2D contactFilter;

    void Awake(){
        playerTransform = transform;
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(interactableLayer);
        contactFilter.useLayerMask = true;
    }

    void Update(){
        DetectClosestInteractable();
        if(Keyboard.current.fKey.wasPressedThisFrame && currentTarget != null)
            currentTarget.Interact();
    }

    void DetectClosestInteractable(){
        int count = Physics2D.OverlapCircle(playerTransform.position, interactionRadius, contactFilter, hitColliders);

        if(count == 0){
            currentTarget = null;
            targetPreview = null;
            return;
        }

        float closestDistance = float.MaxValue;
        IInteractable closestInteractable = null;

        for(int i = 0; i < count; i++){
            IInteractable interactable = hitColliders[i].GetComponent<IInteractable>();
            
            if(interactable != null){
                float distanceSqr = (hitColliders[i].transform.position - playerTransform.position).sqrMagnitude;
                
                if(distanceSqr < closestDistance){
                    closestDistance = distanceSqr;
                    closestInteractable = interactable;
                }
            }
        }

        if(closestInteractable != currentTarget){
            currentTarget = closestInteractable;
            
            if(currentTarget != null) targetPreview = ((MonoBehaviour)currentTarget).gameObject;
            else targetPreview = null;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}