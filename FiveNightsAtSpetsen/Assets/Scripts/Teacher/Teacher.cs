using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Teacher : MonoBehaviour
{
    public TeacherPath path;
    public TeacherMovement movement;
    public List<(string id, float delay)> delays;

    public virtual void ReachedNewWaypoint()
    {
        StartCoroutine(movement.MoveToNext(GetDelay()));
    }

    public float GetDelay()
    {
        TeacherWaypoint waypoint = path.GetWaypoint();

        if (waypoint.id == "")
        {
            return 0;
        }
        else
        {
            foreach ((string id, float delay) item in delays)
            {
                if (item.id == waypoint.id)
                    return item.delay;
            }

            return 0;
        }
    }
}