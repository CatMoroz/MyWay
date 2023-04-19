using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatedText : MonoBehaviour
{
    private TMP_Text textUI;
    private string textDontChanged;
    public bool IfCompleted { get; private set; }
    void Awake()
    {
        textUI = GetComponent<TMP_Text>();
        textDontChanged = textUI.text;
    }
    private void OnEnable()
    {
        StartCoroutine("ShowText", textDontChanged);
    }


    IEnumerator ShowText(string text)
    {
        int i = 0;
        while (i <= text.Length)
        {
            textUI.text = text.Substring(0, i);
            i++;

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        IfCompleted = true;

    gameObject.SetActive(false);
    }
}