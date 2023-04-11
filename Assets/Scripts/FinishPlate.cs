using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPlate : ActivatingBlock
{
    private List<Collider> _collidersOnPlate = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            IsActive.Invoke();
        }
        _collidersOnPlate.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _collidersOnPlate.Remove(other);
        if (other.TryGetComponent<Player>(out Player player))
        {
            IsNotActive.Invoke();
        }
    }
}
