using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : ActivatingBlock
{
    private RaycastHit hitCollider;
    private void OnTriggerStay(Collider other)
    {
        IsActive = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsActive = true;
    }

}
