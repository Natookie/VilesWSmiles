using UnityEngine;
using UnityEngine.InputSystem;
using Nova;

public class RecipeButtonManager : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private GameObject[] panel;
    private int index;
    private int panelLength;

    void Start(){
        panelLength = panel.Length;
    }

    void Update(){
        if(Keyboard.current.tabKey.wasPressedThisFrame) NextRecipe();
        if(Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.fKey.wasPressedThisFrame) ExitRecipe();
    }

    public void NextRecipe(){
        if(panelLength <= 1) return;

        panel[index].SetActive(false);
        index = (index + 1) % panelLength;
        panel[index].SetActive(true);
    }
    public void ExitRecipe() => GameStateManager.Instance.TogglePause();
}
