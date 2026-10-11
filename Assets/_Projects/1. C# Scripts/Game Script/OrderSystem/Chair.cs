using UnityEngine;

public class Chair : MonoBehaviour, IInteractable
{
    public bool IsOccupied { get; private set; }
    public CustomerController CurrentCustomer { get; private set; }
    public float sitYPosition;

    public void Occupy(CustomerController customer){
        IsOccupied = true;
        CurrentCustomer = customer;
    }

    public void Vacate(){
        IsOccupied = false;
        CurrentCustomer = null;
    }

    public void Interact(){
        if(!IsOccupied) return;
        CurrentCustomer.InteractWithCustomer();
    }

    public float GetSitY() => sitYPosition;
}