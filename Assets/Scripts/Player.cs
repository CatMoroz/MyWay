using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private bool _canMove = true;
    private bool _ifPetTaken = false;
    private Vector3 _directionMove;
    private RaycastHit hitCollider;
    private Pet pet;
    [SerializeField] private ControledHeroSwaper _swapControledHero;
    [SerializeField] private int _speed = 2;
    private Vector3 _positionMismatch = new Vector3(0, 0.5f, 0);
    //Because the Player has a higher model then other models its transform.posion is shifted compared to other models
    //so to use raycat you need to change the position of the beggining of the ray to the positions of all
    //models
    private void Update()
    {
        if (_canMove && _swapControledHero.IsControledPlayer)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (_ifPetTaken)
                {
                    TryLowerThePet();
                }
                else
                {
                    TryTakePet();
                }
            }
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
        if (Physics.Raycast(gameObject.transform.position + _positionMismatch, _directionMove, out hitCollider, 1f))
        {

        }
        else
        {
            if (Physics.Raycast(gameObject.transform.position - _positionMismatch, _directionMove, out hitCollider, 1f))
            {
                if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
                {
                    if (moveable.AbilityToMoveObject(direction))
                    {
                        moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                        StartCoroutine(MoveTo(gameObject.transform.position + direction));
                    }
                }
            }
            else
            {
                StartCoroutine(MoveTo(gameObject.transform.position + direction));
            }
        }
    }
    private void TryTakePet()
    {
        if (Physics.Raycast(gameObject.transform.position - _positionMismatch, _directionMove, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Pet>(out Pet pet))
            {
                this.pet = pet;
                _ifPetTaken = true;
                pet.BeTaken(this.transform);
            }
            else if (Physics.Raycast(gameObject.transform.position + _positionMismatch, _directionMove, out hitCollider, 1f))
            {
                if (hitCollider.collider.gameObject.TryGetComponent<Pet>(out Pet pet1))
                {
                    this.pet = pet1;
                    _ifPetTaken = true;
                    pet1.BeTaken(this.transform);
                }
            }
        }
    }
    private void TryLowerThePet()
    {
        if (Physics.Raycast(gameObject.transform.position - _positionMismatch, _directionMove, out hitCollider, 1f))
        {
            if (Physics.Raycast(gameObject.transform.position + _positionMismatch, _directionMove, out hitCollider, 1f))
            {
            }
            else
            {
                _ifPetTaken = false;
                pet.BeLoweredDown(Vector3.up);
                pet = null;
            }
        }
        else
        {
            _ifPetTaken = false;
            pet.BeLoweredDown(Vector3.zero);
            pet = null;
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
