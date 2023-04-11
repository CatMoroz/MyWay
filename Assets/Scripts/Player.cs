using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private bool _canMove = true;
    private bool _ifPetTaken = false;
    private RaycastHit hitCollider;
    private Pet pet;
    [SerializeField] private ControledHeroSwaper _swapControledHero;
    [SerializeField] private EndLevel _endLevel;
    [SerializeField] private int _speed = 2;
    [SerializeField] private int _gravitationSpeed = 5;
    [SerializeField] private int _ForceMovingBlocks = 2;
    private Vector3 _positionMismatch = new Vector3(0, 1f, 0);
    //Because the Player has a higher model then other models its transform.posion is shifted compared to other models
    //so to use raycat you need to change the position of the beggining of the ray to the positions of all
    //models
    private void Awake()
    {
        TryGrounded();
    }
    private void Update()
    {
        if (_canMove && _swapControledHero.IsControledPlayer && _endLevel.IsGameActive)
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
            if (Input.GetKey(KeyCode.W))
            { 
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
                CreatePositionForMovement(transform.forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
                CreatePositionForMovement(transform.forward);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 270, gameObject.transform.rotation.z);
                CreatePositionForMovement(transform.forward);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 90, gameObject.transform.rotation.z);
                CreatePositionForMovement(transform.forward);
            }
        }
    }
    private void CreatePositionForMovement(Vector3 direction)
    {
        if (Physics.Raycast(gameObject.transform.position + _positionMismatch, transform.forward, out hitCollider, 1f))
        {
        }
        else
        {
            if (Physics.Raycast(gameObject.transform.position, transform.forward, out hitCollider, 1f))
            {
                if (hitCollider.collider.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
                {
                    if (moveable.AbilityToMoveObject(direction, _ForceMovingBlocks))
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
        if (Physics.Raycast(gameObject.transform.position, transform.forward, out hitCollider, 1f))
        {
            if (hitCollider.collider.gameObject.TryGetComponent<Pet>(out Pet pet))
            {
                this.pet = pet;
                _ifPetTaken = true;
                pet.BeTaken(this.transform);
            }
            else if (Physics.Raycast(gameObject.transform.position + _positionMismatch, transform.forward, out hitCollider, 1f))
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
        if (Physics.Raycast(gameObject.transform.position, transform.forward, out hitCollider, 1f))
        {
            if (hitCollider.collider.TryGetComponent<Stable>(out Stable stable))
            {
                if (Physics.Raycast(gameObject.transform.position + _positionMismatch, transform.forward, out hitCollider, 1f))
                {
                }
                else
                {
                    _ifPetTaken = false;
                    pet.BeLoweredDown(Vector3.up);
                    pet = null;
                }
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
        _canMove= false;
        while (gameObject.transform.position != target)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, _gravitationSpeed * Time.deltaTime);
            yield return null;
        }
        _canMove = true;
    }
    public void StartCoroutineMoveOnLift(Vector3 NextPosition, float _speed)
    {
        StartCoroutine(MoveOnLift(NextPosition, _speed));
    }
    public void StopCoroutineMoveOnLift(Vector3 NextPosition, float _speed)
    {
        _canMove = true;
        StopAllCoroutines();
    }
    private IEnumerator MoveOnLift(Vector3 NextPosition, float _speed)
    {
        if (!_canMove)
        {
            yield break;
        }
        _canMove = false;
        while (gameObject.transform.position != NextPosition + new Vector3(0, transform.localScale.y / 2, 0))
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, NextPosition + new Vector3(0, transform.localScale.y / 2, 0), _speed * Time.deltaTime);
            yield return null;
        }
        _canMove = true;
    }
}
