using UnityEngine;

public class TaskStepInteractable : Interactable
{
    TaskStep step;

    protected override void Init()
    {
        step = GetComponent<TaskStep>();
    }

    public override void Interact()
    {
        EventsManager.Instance.taskEvents.CompleteStep(step);
    }
}