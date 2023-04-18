using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueFloatingText : MonoBehaviour
{
    [SerializeField] List<FloatedText> TextList = new List<FloatedText>();
    void Update()
    {
        if (TextList.Count>0)
        {
            if (TextList[0].IfCompleted)
            {
                TextList.RemoveAt(0);
                TryStartNext();
            }
        }
    }
    private void TryStartNext()
    {
        if (TextList.Count > 0)
        {
            TextList[0].gameObject.SetActive(true);
        }
    }
}
