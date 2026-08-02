using TMPro;
using UnityEngine;

public class CustomerSceneSetup : MonoBehaviour
{
    [SerializeField] private Chair[] chairs;
    [SerializeField] private Transform spawnPoint;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI debt;

    void Start(){
        if(CustomerManager.Instance != null){
            CustomerManager.Instance.SetupSceneReferences(chairs, spawnPoint);
            GameManager.Instance.SetupDebtText(debt);
        }
    }
}