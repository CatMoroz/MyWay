using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinPlate : ActivatingBlock
{
    private List<Collider> _collidersOnPlate = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Pet>(out Pet pet))
        {
            StartWork.Invoke(true);
        }
        _collidersOnPlate.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _collidersOnPlate.Remove(other);
        if (other.TryGetComponent<Pet>(out Pet pet))
        {
            StartWork.Invoke(false);
        }
    }
}
