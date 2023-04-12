using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private ActivatingBlock[] _activatingBlocks;
    [SerializeField] private LevelControler _levelControler;
    [SerializeField] private GameObject FixedButton;

    private int _activedActivatingBlocks;
    public bool IsGameActive { get; private set; } = true;

    public void PlusActiveBlock()
    {
        _activedActivatingBlocks++;
        if (_activedActivatingBlocks == _activatingBlocks.Length)
        {
            IsGameActive = false;
            FixedButton.SetActive(false);
            _levelControler.LevelComplete();
            gameObject.SetActive(true);
        }
    }
    public void MinusActiveBlock()
    {
        _activedActivatingBlocks--;
    }
}
