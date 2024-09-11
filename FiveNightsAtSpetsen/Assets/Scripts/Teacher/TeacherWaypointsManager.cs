using UnityEngine;
using System.Collections.Generic;

public class TeacherWaypointsManager : MonoBehaviour
{
    public List<TeacherWaypoint> waypoints = new();

    void OnValidate()
    {
        waypoints.Clear();

        foreach (Transform child in transform)
            waypoints.Add(child.GetComponent<TeacherWaypoint>());
    }
}
