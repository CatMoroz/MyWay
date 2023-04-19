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
    [SerializeField] private GameObject TuturialText;
    private int _levelCompleted;
    public void LevelComplete()
    {
        _levelCompleted = PlayerPrefs.GetInt("LevelCompleted");
        if (SceneManager.GetActiveScene().buildIndex <= _levelCompleted)
        {
            NextLevelButton.interactable = true;
        }
        else
        {
            NextLevelButton.interactable = false;
        }
        LevelStatusText.text = "Уровень пройден";
        NextLevelButton.gameObject.SetActive(true);
        ContinueLevelButton.gameObject.SetActive(false);
        LoseLevelButton.gameObject.SetActive(false);
        if (TuturialText!=null)
        {
            TuturialText.SetActive(false);
        }
    }
    public void OnPause()
    {
        LevelStatusText.text = "Пауза";
        NextLevelButton.gameObject.SetActive(false);
        ContinueLevelButton.gameObject.SetActive(true);
        LoseLevelButton.gameObject.SetActive(false);
        if (TuturialText != null)
        {
            TuturialText.SetActive(false);
        }
    }
    public void Lose()
    {
        LevelStatusText.text = "Поражение";
        NextLevelButton.gameObject.SetActive(false);
        ContinueLevelButton.gameObject.SetActive(false);
        LoseLevelButton.gameObject.SetActive(true);
        if (TuturialText != null)
        {
            TuturialText.SetActive(false);
        }
    }
}
