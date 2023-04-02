using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private bool _canMove = true;
    private Vector3 _directionMove;
    private RaycastHit hitCollider;
    [SerializeField] private ControledHeroSwaper _swapControledHero;
    [SerializeField] private int _speed = 2;
    private void Update()
    {
        Debug.DrawRay(gameObject.transform.position, _directionMove, Color.red);
        if (_canMove && _swapControledHero.IsControledPlayer)
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
        if (Physics.Raycast(gameObject.transform.position - new Vector3(0, 0.5f, 0), _directionMove, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                if (moveable.AbilityToMoveObject(direction))
                {
                    moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                    StartCoroutine(MoveTo(gameObject.transform.position + direction));
                }
            }
            hitCollider = new RaycastHit();
        }
        else
        {
            StartCoroutine(MoveTo(gameObject.transform.position + direction));
        }
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
