using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    private RaycastHit hitCollider;
    private Vector3 direction;
    [SerializeField] private int _speed = 2;
    [SerializeField] private int _gravitationSpeed = 5;
    private void Awake()
    {
        TryGrounded();
    }
    public bool AbilityToMoveObject(Vector3 direction)
    {
        this.direction = direction;
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hitCollider, 1f))
        {
            return false;
        }
        else if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
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
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hitCollider, 1f))
        {

        }
        else if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
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
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
        TryGrounded();
    }
    private void TryGrounded()
    {
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hitCollider, 10f))
        {
            if (hitCollider.collider.gameObject.transform.position.y + hitCollider.collider.gameObject.transform.localScale.y / 2 < transform.position.y - transform.localScale.y / 2)
            {
                Vector3 GroudedPosition = new Vector3(transform.position.x,
                    hitCollider.collider.gameObject.transform.position.y + hitCollider.collider.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2,
                    transform.position.z);
                StartCoroutine(Gravitation(GroudedPosition));
            }
        }
    }
    private IEnumerator Gravitation(Vector3 target)
    {
        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, _gravitationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
