using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private ActivatingBlock[] _activatingBlocks;

    [SerializeField] private float _floorChange;
    [SerializeField] private int _speed = 3;

    private Vector3 _transformInActive;
    private Vector3 _transformOutActive;
    private int _activedActivatingBlocks;

    private void Awake()
    {
        _transformOutActive = transform.position;
        _transformInActive = transform.position - new Vector3(0, _floorChange, 0);
    }

    public void ActivatingBlockUsed(bool activeOrNot)
    {
        if (activeOrNot)
        {
            _activedActivatingBlocks++;
            if (_activedActivatingBlocks == _activatingBlocks.Length)
            {
                StopAllCoroutines();
                StartCoroutine(WallGoes(_transformInActive));
            }
        }
        else
        {
            _activedActivatingBlocks--;
            if (_activedActivatingBlocks + 1 == _activatingBlocks.Length)
            {
                StopAllCoroutines();
                StartCoroutine(WallGoes(_transformOutActive));
            }
        }
    }
    private IEnumerator WallGoes(Vector3 NextPosition)
    {
        while (gameObject.transform.position != NextPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, NextPosition, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
