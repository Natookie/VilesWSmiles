using UnityEngine;
using Nova;
using System;

public class DrinkStatusUI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private SlidingPanel panel;
    [SerializeField] private TextBlock statusText;

    void Start(){
        DrinkMaker.Instance.OnDrinkComplete += ShowStatus;
        DrinkMaker.Instance.OnDrinkServed += HideStatus;
    }

    void ShowStatus(){
        int failureCount = DrinkMaker.Instance.GetFailureCount();
        string qualityText = failureCount switch{
            0 => "Perfect",
            1 => "Good",
            2 => "Okay",
            _ => "Poor"
        };
        statusText.Text = $"Drink complete ({qualityText})! Click on the customer to serve.";
        panel.ShowPanel();
    }

    void HideStatus(CustomerController customer, bool success) => panel.HidePanel();
    void OnDestroy(){
        DrinkMaker.Instance.OnDrinkComplete -= ShowStatus;
        DrinkMaker.Instance.OnDrinkServed -= HideStatus;
    }
}