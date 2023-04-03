using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    private bool _canMove = true;
    private Vector3 _directionMove;
    private RaycastHit hitCollider;
    [SerializeField] private ControledHeroSwaper _swapControledHero;
    [SerializeField] private int _speed = 2;
    private void Update()
    {
        if (_canMove && _swapControledHero.IsControledPet)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
                _directionMove = Vector3.forward;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
                _directionMove = Vector3.back;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 270, gameObject.transform.rotation.z);
                _directionMove = Vector3.left;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 90, gameObject.transform.rotation.z);
                _directionMove = Vector3.right;
                CreatePositionForMovement(_directionMove);
            }
        }
    }
    private void CreatePositionForMovement(Vector3 direction)
    {
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.125f, 0), _directionMove, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                if (moveable.AbilityToMoveObject(direction))
                {
                    moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                    StartCoroutine(MoveTo(gameObject.transform.position + direction));
                }
            }
            else if (hitCollider.collider.gameObject.TryGetComponent<Arch>(out Arch arch))
            {
                if (arch.AbilityToMoveThroughHole(direction))
                {
                    StartCoroutine(MoveTo(gameObject.transform.position + 2 * direction));
                }
            }
        }
        else
        {
            StartCoroutine(MoveTo(gameObject.transform.position + direction));
        }
    }
    public void BeTaken(Transform parent)
    {
        _canMove = false;
        this.transform.position = new Vector3(parent.position.x + (parent.localScale.x / 2 + this.transform.localScale.x / 2) * parent.forward.x,
            parent.position.y,
            parent.position.z + (parent.localScale.z / 2 + this.transform.localScale.z / 2) * parent.forward.z);
        this.transform.SetParent(parent);
    }
    public void BeLoweredDown(Vector3 newPositionYBeforeCalculated)
    {
        Vector3 newPosition = new Vector3(this.transform.parent.position.x + this.transform.parent.forward.x,
            newPositionYBeforeCalculated.y,
            this.transform.parent.position.z + this.transform.parent.forward.z);
        this.transform.parent = null;
        this.transform.position = newPosition + new Vector3(0, this.transform.localScale.y / 2, 0);
        _canMove = true;
    }
    private IEnumerator MoveTo(Vector3 target)
    {
        _canMove = false;
        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
        _canMove = true;
    }
}