using UnityEngine;
using System.Collections.Generic;

public class TeacherPath : MonoBehaviour
{
    public TeacherWaypointsManager waypoints;
    public List<int> path = new() { 0, 1, 4, 6, 1 };
    public PathingType pathing = PathingType.Repeating;
    public int pathIndex = 0;
    int pathIndexMod = 1;

    public Color lineColor = Color.yellow;

    public void Next()
    {
        if (pathing == PathingType.Reversing)
        {
            if ((pathIndex >= path.Count - 1 && pathIndexMod > 0) || (pathIndex == 0 && pathIndexMod < 0))
                pathIndexMod *= -1;
        }
        else if (pathing == PathingType.Repeating)
        {
            if (pathIndex >= path.Count - 1)
                pathIndex = -1;
        }

        pathIndex += pathIndexMod;
    }

    public Vector3 GetPos()
    {
        return waypoints.waypoints[path[pathIndex]].transform.position;
    }

    public TeacherWaypoint GetWaypoint()
    {
        return waypoints.waypoints[path[pathIndex]];
    }

    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.waypoints.Contains(null)) return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 start = GizmoGetPos(i);
            Vector3 end = GizmoGetPos(i + 1);

            Gizmos.color = lineColor;

            Gizmos.DrawLine(start, end);
        }
    }

    Vector3 GizmoGetPos(int i)
    {
        return waypoints.waypoints[path[i]].transform.position;
    }
}