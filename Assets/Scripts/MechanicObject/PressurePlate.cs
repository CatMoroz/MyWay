using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : ActivatingBlock
{
    private List<Collider> _collidersOnPlate = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (_collidersOnPlate.Count == 0)
        {
            IsActive.Invoke();
        }
        _collidersOnPlate.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _collidersOnPlate.Remove(other);
        if (_collidersOnPlate.Count==0)
        {
            IsNotActive.Invoke();
        }
    }

}
