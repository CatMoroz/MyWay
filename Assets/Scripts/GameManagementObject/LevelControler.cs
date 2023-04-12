using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControler : MonoBehaviour
{
    public static LevelControler Instance { get; private set;} = null;
    private int _sceneIndex;
    private int _levelCompleted;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        _levelCompleted = PlayerPrefs.GetInt("LevelCompleted");
    }
    public void LevelComplete()
    {
        if (_levelCompleted <= _sceneIndex)
        {
            PlayerPrefs.SetInt("LevelCompleted", _sceneIndex);
        }
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void OpenLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void OpenNextLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelCompleted")+1);
    }
}
