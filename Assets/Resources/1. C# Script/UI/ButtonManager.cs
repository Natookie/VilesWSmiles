using UnityEngine;
using Nova;
using NaughtyAttributes;

public class ButtonManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [ReadOnly] public AdditiveSceneManager asm;

    void Awake(){
        asm ??= AdditiveSceneManager.Instance;
    }

    public void OnClickButton1() => GameStateManager.Instance.StartGame("GameScene");
    public void OnClickButton2() => asm.LoadScene("ShopScene");
    public void OnClickButton3(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
