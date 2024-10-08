using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskUIManager : MonoBehaviour
{
    public RectTransform rect;
    TextMeshProUGUI tmp;

    void Awake()
    {
        tmp = rect.Find("Info").GetComponent<TextMeshProUGUI>();
    }

    public void Init(string initialString)
    {
        tmp.text = initialString;
        SetRect();
    }

    public void UpdateTask(TaskStep lastStep, TaskStep nextStep)
    {
        if (nextStep != null)
        {
            string completionString = (lastStep.stepIndex == nextStep.stepIndex)
                                    ? lastStep.stepCompletionString : lastStep.indexCompletionString;

            completionString += (completionString == "") ? "" : "\n\n";

            tmp.text = completionString + nextStep.taskString;
            StartCoroutine(RemoveCompletionText(nextStep.taskString));
        }
        else
        {
            tmp.text = "Alla uppgifter utförda!!";
        }
        SetRect();
    }

    IEnumerator RemoveCompletionText(string taskString)
    {
        yield return new WaitForSeconds(5f);

        tmp.text = taskString;
        SetRect();
    }

    void SetRect()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tmp.preferredHeight + 50 + 20);
    }
}