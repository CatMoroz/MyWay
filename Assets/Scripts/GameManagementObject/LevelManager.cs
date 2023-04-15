using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ActivatingBlock[] _EndLevelActivatingBlocks;
    [SerializeField] private LevelControler _levelControler;
    [SerializeField] private StopLevel _stopLevelWindow;

    [SerializeField] private Button _pauseLevelButton;
    [SerializeField] private Button _resetLevelButton;
    [SerializeField] private Button _swapHeroButton;

    private int _activedActivatingBlocks;
    public bool IsGameActive { get; private set; } = true;

    public void PlusActivatedBlock()
    {
        _activedActivatingBlocks++;
        if (_activedActivatingBlocks == _EndLevelActivatingBlocks.Length)
        {
            LevelComplete();
        }
    }
    public void MinusActivatedBlock()
    {
        _activedActivatingBlocks--;
    }
    public void OnPause()
    {
        IsGameActive = false;
        _stopLevelWindow.OnPause();
        _stopLevelWindow.gameObject.SetActive(true);
        _swapHeroButton.gameObject.SetActive(false);
        _pauseLevelButton.gameObject.SetActive(false);
        _resetLevelButton.gameObject.SetActive(false);
    }
    public void OffPause()
    {
        IsGameActive = true;
        _stopLevelWindow.gameObject.SetActive(false);
        _swapHeroButton.gameObject.SetActive(true);
        _pauseLevelButton.gameObject.SetActive(true);
        _resetLevelButton.gameObject.SetActive(true);
    }
    public void Lose()
    {
        IsGameActive = false;
        _stopLevelWindow.Lose();
        _stopLevelWindow.gameObject.SetActive(true);
        _swapHeroButton.gameObject.SetActive(false);
        _pauseLevelButton.gameObject.SetActive(false);
        _resetLevelButton.gameObject.SetActive(false);
    }
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LevelComplete()
    {
        IsGameActive = false;
        _stopLevelWindow.LevelComplete();
        _stopLevelWindow.gameObject.SetActive(true);
        _swapHeroButton.gameObject.SetActive(false);
        _pauseLevelButton.gameObject.SetActive(false);
        _resetLevelButton.gameObject.SetActive(false);
        _levelControler.LevelComplete();
    }
}
