using UnityEngine;
using TMPro;

public class ClueUI : MonoBehaviour
{
    public TaskManager taskManager;
    TextMeshProUGUI tmp;
    bool clueShown;

    void OnEnable()
    {
        EventsManager.Instance.taskEvents.OnStepCompletionAccepted += StepCompleted;
    }

    void OnDisable()
    {
        EventsManager.Instance.taskEvents.OnStepCompletionAccepted -= StepCompleted;
    }

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            ToggleClue();
    }

    void ToggleClue()
    {
        if (clueShown)
        {
            tmp.text = "";
            clueShown = false;
            return;
        }

        string clueString = taskManager.steps[0].clueString;

        if (clueString == "")
            tmp.text = "Ingen ledtråd tillgänglig!";
        else
            tmp.text = clueString;


        clueShown = true;
    }

    void StepCompleted(TaskStep lastStep, TaskStep nextStep)
    {
        tmp.text = "";
    }
}