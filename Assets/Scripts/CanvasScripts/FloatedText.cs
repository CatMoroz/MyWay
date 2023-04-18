using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatedText : MonoBehaviour
{
    private TMP_Text textUI;
    public bool IfCompleted { get; private set; }
    void Start()
    {
        textUI = GetComponent<TMP_Text>();
        StartCoroutine("ShowText", textUI.text);
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