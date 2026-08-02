using UnityEngine;

public class Book : MonoBehaviour, IInteractable
{
    public void Interact(){
        DialogueManager.Instance.TriggerInteraction("OpenRecipeBook");
        GameStateManager.Instance.OpenRecipe();
    }
}