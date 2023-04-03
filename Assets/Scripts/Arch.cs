using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arch : MonoBehaviour
{
    private RaycastHit hitCollider;
    public bool AbilityToMoveThroughHole(Vector3 direction)
    {
        if ((transform.forward == direction || transform.forward == -1 * direction))
        {
            if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
            {
                if (hitCollider.collider.gameObject.TryGetComponent<Pet>(out Pet pet))
                {
                    return true;
                }
                else
                { 
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
