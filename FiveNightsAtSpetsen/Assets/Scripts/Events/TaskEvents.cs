using System;

public class TaskEvents
{
    public event Action<TaskStep> OnStepCompleted;
    public void CompleteStep(TaskStep step)
    {
        OnStepCompleted?.Invoke(step);
    }

    public event Action<TaskStep, TaskStep> OnStepCompletionAccepted;
    public void AcceptStepCompletion(TaskStep lastStep, TaskStep nextStep) 
    {
        OnStepCompletionAccepted?.Invoke(lastStep, nextStep);
    }

    public event Action OnAllTasksCompleted;
    public void CompletedAllTasks()
    {
        OnAllTasksCompleted?.Invoke();
    }
}