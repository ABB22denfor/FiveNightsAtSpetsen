using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskManager : MonoBehaviour
{
    public List<TaskStep> steps = new();
    public Color lineColor = Color.blue;
    public Color firstColor = Color.yellow;
    public Color lastColor = Color.red;

    TaskUIManager ui;

    void OnEnable()
    {
        EventsManager.Instance.taskEvents.OnStepCompleted += StepCompleted;
    }

    void OnDisable()
    {
        EventsManager.Instance.taskEvents.OnStepCompleted -= StepCompleted;
    }

    void Start() 
    {
        steps = steps.OrderBy(s => s.stepIndex).ToList();
        ui = GetComponent<TaskUIManager>();
    }

    void StepCompleted(TaskStep step)
    {
        if (steps[0].stepIndex == step.stepIndex)
        {
            steps.Remove(step);
            Debug.Log("Completed step " + step.id);
            Destroy(step.gameObject);

            if (steps.Count == 0)
            {
                EventsManager.Instance.taskEvents.CompletedAllTasks();
            }
            else
            {
                foreach (TaskStep ts in steps) {
                    if (ts.stepIndex == steps[0].stepIndex) {
                        ts.gameObject.SetActive(true);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Attempted to complete step " + step.id + 
                      " out of order\n(Current step index: " + steps[0].stepIndex +
                      ", attempted to complete step of index " + step.stepIndex);
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