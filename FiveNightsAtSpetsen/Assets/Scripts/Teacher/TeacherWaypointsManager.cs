using UnityEngine;
using System.Collections.Generic;

public class TeacherWaypointsManager : MonoBehaviour
{
    public List<TeacherWaypoint> waypoints = new();

    void OnDrawGizmos()
    {
        foreach (TeacherWaypoint waypoint in waypoints)
        {
            Gizmos.color = (waypoint.id == "") ? Color.blue : Color.red;

            Gizmos.DrawSphere(waypoint.transform.position, (waypoint.id == "") ? 0.2f : 0.25f);
        }
    }
}
