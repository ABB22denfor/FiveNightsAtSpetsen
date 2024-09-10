using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public List<TaskStep> steps = new();
    public Color lineColor = Color.blue;
    public Color firstColor = Color.yellow;
    public Color lastColor = Color.red;

    void OnEnable()
    {
        EventsManager.Instance.taskEvents.OnStepCompleted += StepCompleted;
    }

    void OnDisable()
    {
        EventsManager.Instance.taskEvents.OnStepCompleted -= StepCompleted;
    }

    void OnValidate()
    {
        steps.Clear();

        foreach (Transform goStep in transform)
            steps.Add(goStep.GetComponent<TaskStep>());
    }

    void StepCompleted(TaskStep step)
    {
        if (steps[0] == step)
        {
            steps.RemoveAt(0);
            Debug.Log("Completed step " + step.id);
            Destroy(step.gameObject);

            if (steps.Count == 0)
                Debug.Log("All task steps completed");
        }
        else
        {
            Debug.Log("Attempted to complete step " + step.id + " out of order");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (steps == null) return;

        for (int i = 0; i < steps.Count - 1; i++)
        {
            Vector3 start = steps[i].transform.position;
            Vector3 end = steps[i + 1].transform.position;

            Gizmos.color = (i == 0 ? firstColor : (i == steps.Count - 2 ? lastColor : lineColor));

            Gizmos.DrawLine(start, end);
        }
    }
}