using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StopLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text LevelStatusText;
    [SerializeField] private Button NextLevelButton;
    [SerializeField] private Button ContinueLevelButton;
    [SerializeField] private Button LoseLevelButton;
    private int _levelCompleted;
    private void OnEnable()
    {
        _levelCompleted = PlayerPrefs.GetInt("LevelCompleted");
        if (SceneManager.GetActiveScene().buildIndex<= _levelCompleted)
        {
            NextLevelButton.interactable = true;
        }
        else
        {
            NextLevelButton.interactable = false;
        }
    }
    public void LevelComplete()
    {
        LevelStatusText.text = "������� �������";
        NextLevelButton.gameObject.SetActive(true);
        ContinueLevelButton.gameObject.SetActive(false);
        LoseLevelButton.gameObject.SetActive(false);
    }
    public void OnPause()
    {
        LevelStatusText.text = "�����";
        NextLevelButton.gameObject.SetActive(false);
        ContinueLevelButton.gameObject.SetActive(true);
        LoseLevelButton.gameObject.SetActive(false);
    }
    public void Lose()
    {
        LevelStatusText.text = "���������";
        NextLevelButton.gameObject.SetActive(false);
        ContinueLevelButton.gameObject.SetActive(false);
        LoseLevelButton.gameObject.SetActive(true);
    }
}
