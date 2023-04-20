using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pet : MonoBehaviour
{
    private bool _canMove = true;
    private Vector3 _directionMove;
    private RaycastHit hitCollider;
    private Outline _outline;

    [SerializeField] private ControledHeroSwaper _swapControledHero;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private int _speed = 2;
    [SerializeField] private int _gravitationSpeed = 5;
    [SerializeField] private int _ForceMovingBlocks = 1;

    public bool IsTaken { get; private set; }
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        TryGrounded();
    }
    private void Update()
    {
        if (_swapControledHero.IsControledPet)
        {
            _outline.enabled = true;
        }
        else
        {
            _outline.enabled = false;
        }
        if (_canMove && _swapControledHero.IsControledPet && _levelManager.IsGameActive)
        {
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
                _directionMove = Vector3.forward;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
                _directionMove = Vector3.back;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 270, gameObject.transform.rotation.z);
                _directionMove = Vector3.left;
                CreatePositionForMovement(_directionMove);
            }
            else if (Input.GetKey(KeyCode.D))
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
                if (moveable.AbilityToMoveObject(direction, _ForceMovingBlocks))
                {
                    moveable.MoveObjectTo(moveable.gameObject.transform.position + direction, direction);
                    StartCoroutine(MoveTo(gameObject.transform.position + direction));
                }
            }
            else if (hitCollider.collider.gameObject.TryGetComponent<Arch>(out Arch arch))
            {
                int LengthHole = 0;
                if (arch.AbilityToMoveThroughHole(direction, ref LengthHole))
                {
                    StartCoroutine(MoveTo(gameObject.transform.position + LengthHole * direction));
                }
            }
            else if (hitCollider.collider.gameObject.TryGetComponent<CabinHole>(out CabinHole cabinHole))
            {
                StartCoroutine(MoveTo(gameObject.transform.position + direction));
            }
        }
        else
        {
            StartCoroutine(MoveTo(gameObject.transform.position + direction));
        }
    }
    public void BeTaken(Transform parent)
    {
        IsTaken = true;
        _canMove = false;
        this.transform.position = new Vector3(parent.position.x + (parent.localScale.x / 2 + this.transform.localScale.x / 2) * parent.forward.x,
            parent.position.y + 0.5f,
            parent.position.z + (parent.localScale.z / 2 + this.transform.localScale.z / 2) * parent.forward.z);
        this.transform.SetParent(parent);
    }
    public void BeLoweredDown(Vector3 newPositionYBeforeCalculated)
    {
        Vector3 newPosition = new Vector3(this.transform.parent.position.x + this.transform.parent.forward.x,
            newPositionYBeforeCalculated.y + this.transform.parent.position.y - 0.5f,
            this.transform.parent.position.z + this.transform.parent.forward.z);
        this.transform.parent = null;
        this.transform.position = newPosition + Vector3.up/2;
        IsTaken = false;
        _canMove = true;
        TryGrounded();
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
                        hitCollider.collider.gameObject.transform.position.y + hitCollider.collider.gameObject.transform.localScale.y / 2 + transform.localScale.y *1.33f/ 2,
                        transform.position.z);
                    StartCoroutine(Gravitation(GroudedPosition));
                }
            }
        }
    }
    private IEnumerator Gravitation(Vector3 target)
    {
        _canMove = false;
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
        while (gameObject.transform.position != NextPosition + Vector3.up / 2)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, NextPosition + Vector3.up / 2, _speed * Time.deltaTime);
            yield return null;
        }
        _canMove = true;
    }
}
