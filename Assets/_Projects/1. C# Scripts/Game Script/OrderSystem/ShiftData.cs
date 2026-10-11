using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewShift", menuName = "Customer/Shift Data")]
public class ShiftData : ScriptableObject
{
    public int shiftNumber;
    public int customersToServe;
    public List<CustomerSpawnEntry> spawnSequence;
    public bool usePreDeterminedCustomers = true;
    
    [System.Serializable]
    public class CustomerSpawnEntry
    {
        public string firstName;
        public string lastName;
        public float delayBeforeSpawn;
    }
}