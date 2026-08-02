using UnityEngine;
using Nova;
using NaughtyAttributes;

public class CameraAssigner : MonoBehaviour
{
    private ScreenSpace sp;
    void OnEnable() => Assign();

    [Button("assign")]
    public void Assign(){
        sp = GetComponent<ScreenSpace>();
        if(sp.TargetCamera == null) sp.TargetCamera = Camera.main;
    }
}