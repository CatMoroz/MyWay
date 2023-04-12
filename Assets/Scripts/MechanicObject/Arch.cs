using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arch : MonoBehaviour
{
    private RaycastHit hitCollider;
    public bool AbilityToMoveThroughHole(Vector3 direction, ref int LengthHole)
    {
        if ((transform.forward == direction || transform.forward == -1 * direction))
        {
            if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
            {
                if (hitCollider.collider.gameObject.TryGetComponent<Arch>(out Arch arch))
                {
                    LengthHole++;
                    return arch.AbilityToMoveThroughHole(direction, ref LengthHole);
                }
                else
                {
                    LengthHole = 0;
                    return false;
                }
            }
            else
            {
                LengthHole+=2;
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
