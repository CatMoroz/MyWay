using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    private RaycastHit hitCollider;
    private Vector3 direction;
    private void Update()
    {
        Debug.DrawRay(gameObject.transform.position, direction, Color.red);
    }
    public bool AbilityToMoveObject(Vector3 direction)
    {
        this.direction = direction;
        if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                return moveable.AbilityToMoveObject(direction);
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
    public void MoveObjectTo(Vector3 target, Vector3 direction)
    {
        this.direction = direction;
        if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                StartCoroutine(MoveObjectToCoroutine(target));
            }
            hitCollider = new RaycastHit();
        }
        else
        {
            StartCoroutine(MoveObjectToCoroutine(target));
        }
    }
    private IEnumerator MoveObjectToCoroutine(Vector3 target)
    {
        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, 2 * Time.deltaTime);
            yield return null;
        }
    }
}
