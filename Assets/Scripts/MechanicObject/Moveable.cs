using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    private RaycastHit hitCollider;
    private Vector3 direction;
    private bool _moveOnLift;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private int _speed = 2;
    [SerializeField] private int _gravitationSpeed = 5;
    [SerializeField] private int _requiredForceToMove;
    private void Awake()
    {
        TryGrounded();
    }
    public bool AbilityToMoveObject(Vector3 direction, int Force)
    {
        this.direction = direction;
        if (Force < _requiredForceToMove)
        {
            return false;
        }
        if (_moveOnLift)
        {
            return false;
        }
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hitCollider, 1f))
        {
            if (hitCollider.collider.TryGetComponent<Pet>(out Pet pet))
            {
                if (pet.IsTaken)
                {
                    if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
                    {
                        if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
                        {
                            return moveable.AbilityToMoveObject(direction, Force);
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
            return false;
        }
        else if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                return moveable.AbilityToMoveObject(direction, Force);
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
        if (Physics.Raycast(gameObject.transform.position, direction, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                StartCoroutine(MoveObjectToCoroutine(target));
            }
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
            if (hitCollider.collider.gameObject.TryGetComponent<Player>(out Player player))
            {
                if (hitCollider.collider.gameObject.transform.position.y + 1.5f < transform.position.y - transform.localScale.y / 2)
                {
                    Vector3 GroudedPosition = new Vector3(transform.position.x,
                        hitCollider.collider.gameObject.transform.position.y + 1.5f + transform.localScale.y / 2,
                        transform.position.z);
                    StartCoroutine(Gravitation(GroudedPosition));
                }
                else
                {
                    _levelManager.Lose();
                }
            }
            else
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
    }
    private IEnumerator Gravitation(Vector3 target)
    {
        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, _gravitationSpeed * Time.deltaTime);
            yield return null;
        }
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Stable>(out Stable stable))
            {

            }
            else
            {
                _levelManager.Lose();
            }
        }
    }
    public void StartCoroutineMoveOnLift(Vector3 NextPosition, float _speed)
    {
        StartCoroutine(MoveOnLift(NextPosition, _speed));
    }
    public void StopCoroutineMoveOnLift(Vector3 NextPosition, float _speed)
    {
        StopAllCoroutines();
    }
    private IEnumerator MoveOnLift(Vector3 NextPosition, float _speed)
    {
        _moveOnLift = true;
        while (gameObject.transform.position != NextPosition + new Vector3(0, transform.localScale.y / 2, 0))
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, NextPosition + new Vector3(0, transform.localScale.y / 2, 0), _speed * Time.deltaTime);
            yield return null;
        }
        _moveOnLift = false;
    }
}
