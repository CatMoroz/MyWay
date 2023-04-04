using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lift : MonoBehaviour
{
    [SerializeField] private ActivatingBlock[] _activatingBlocks;

    [SerializeField] private float _floorChange;
    [SerializeField] private int _speed = 3;

    private List<Collider> _blocksOnLift = new List<Collider>();
    private Vector3 _transformInActive;
    private Vector3 _transformOutActive;
    private RaycastHit BlockOnLift;
    private bool _isActive;
    private bool _inProgress;

    private void Awake()
    {
        _transformOutActive = transform.position;
        _transformInActive = transform.position + new Vector3(0, _floorChange, 0);
    }
    private void Update()
    {
        foreach ( var item in _activatingBlocks)
        {
            if (!item.IsActive)
            {
                _isActive = false;
                break;
            }
            _isActive = true;
        }
        if (!_inProgress)
        {
            StartCoroutine(Work(_isActive));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _blocksOnLift.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _blocksOnLift.Remove(other);
    }
    private IEnumerator Work(bool WhereGo)
    {
        _inProgress = true;
        if (WhereGo)
        {
            while (gameObject.transform.position != _transformInActive)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _transformInActive, _speed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (gameObject.transform.position != _transformOutActive)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _transformOutActive, _speed * Time.deltaTime);
                yield return null;
            }
        }
        _inProgress = false;
    }

}
