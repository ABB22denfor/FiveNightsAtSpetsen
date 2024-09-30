using System;

public class TaskEvents
{
    public event Action<TaskStep> OnStepCompleted;
    public void CompleteStep(TaskStep step)
    {
        OnStepCompleted?.Invoke(step);
    }

    public event Action OnAllTasksCompleted;
    public void CompletedAllTasks()
    {
        OnAllTasksCompleted?.Invoke();
    }
}