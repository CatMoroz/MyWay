using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lift : MonoBehaviour
{
    [SerializeField] private ActivatingBlock[] _activatingBlocks;
    [SerializeField] private LevelManager _levelManager;

    [SerializeField] private float _floorChange;
    [SerializeField] private int _speed = 3;

    private Collider _blocksOnLift;
    private Vector3 _transformInActive;
    private Vector3 _transformOutActive;
    private int _activedActivatingBlocks;

    private void Awake()
    {
        if (_levelManager == null)
        {
            Debug.Log("lif");
        }
        _transformOutActive = transform.position;
        _transformInActive = transform.position + new Vector3(0, _floorChange, 0);
    }
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.position.y< transform.position.y)
        {
            if (other.gameObject.TryGetComponent<Player>(out Player player))
            {
                _levelManager.Lose();
            }
            else if (other.gameObject.TryGetComponent<Pet>(out Pet pet))
            {
                _levelManager.Lose();
            }
            else if (other.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            _blocksOnLift = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_blocksOnLift == other)
        {
            _blocksOnLift = null;
        }
    }

    
    public void ActivatingBlockUsed(bool ActivatingBlockStatus)
    {
        if (ActivatingBlockStatus)
        {
            _activedActivatingBlocks++;
            if (_activedActivatingBlocks == _activatingBlocks.Length)
            {
                if (_blocksOnLift != null)
                {
                    MoveInActive();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(LiftGoes(_transformInActive));
                }
            }
        }
        else
        {
            _activedActivatingBlocks--;
            if (_activedActivatingBlocks + 1 == _activatingBlocks.Length)
            {
                if (_blocksOnLift != null)
                {
                    MoveOutActive();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(LiftGoes(_transformOutActive));
                }
            }
        }
    }
    private void MoveInActive()
    {
        if (_blocksOnLift.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
        {
            moveable.StopCoroutineMoveOnLift(_transformOutActive, _speed);
            moveable.StartCoroutineMoveOnLift(_transformInActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformInActive));
        }
        else if (_blocksOnLift.gameObject.TryGetComponent<Pet>(out Pet pet))
        {
            pet.StopCoroutineMoveOnLift(_transformOutActive, _speed);
            pet.StartCoroutineMoveOnLift(_transformInActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformInActive));
        }
        else if (_blocksOnLift.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.StopCoroutineMoveOnLift(_transformOutActive, _speed);
            player.StartCoroutineMoveOnLift(_transformInActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformInActive));
        }
    }
    private void MoveOutActive()
    {
        if (_blocksOnLift.gameObject.TryGetComponent<Moveable>(out Moveable moveable))
        {
            moveable.StopCoroutineMoveOnLift(_transformInActive, _speed);
            moveable.StartCoroutineMoveOnLift(_transformOutActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformOutActive));
        }
        else if (_blocksOnLift.gameObject.TryGetComponent<Pet>(out Pet pet))
        {
            pet.StopCoroutineMoveOnLift(_transformInActive, _speed);
            pet.StartCoroutineMoveOnLift(_transformOutActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformOutActive));
        }
        else if (_blocksOnLift.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.StopCoroutineMoveOnLift(_transformInActive, _speed);
            player.StartCoroutineMoveOnLift(_transformOutActive, _speed);
            StopAllCoroutines();
            StartCoroutine(LiftGoes(_transformOutActive));
        }
    }
    private IEnumerator LiftGoes(Vector3 NextPosition)
    {
        while (gameObject.transform.position != NextPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, NextPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
