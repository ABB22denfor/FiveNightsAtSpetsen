using UnityEngine;
using System.Collections.Generic;

public class TeacherWaypointsManager : MonoBehaviour
{
    public List<TeacherWaypoint> waypoints = new();

    private void OnValidate()
    {
        waypoints.Clear();

        foreach (Transform child in gameObject.transform)
        {
            waypoints.Add(child.GetComponent<TeacherWaypoint>());
        }
    }
}
