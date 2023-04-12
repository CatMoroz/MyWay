using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private List<Button> buttonList;
    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (PlayerPrefs.GetInt("LevelCompleted") >= i)
            {
                buttonList[i].interactable = true;
            }
            else
            {
                buttonList[i].interactable = false;
            }
        }
    }
}
