using UnityEngine;
using System.Collections.Generic;

public class TeacherRoomPath : MonoBehaviour
{
    public TeacherPathManager manager;
    public List<int> path = new() { 0, 1, 2, 3, 1 };
    public int exitPoint = 5;
    public int pathIndex = 0;

    public Color lineColor = Color.blue;
    public Color exitColor = Color.red;

    public void Next()
    {
        if (pathIndex >= path.Count - 1)
        {
            pathIndex = -1;
            manager.RoomFinished(this);
        }

        pathIndex++;
    }

    public Vector3 GetPos()
    {
        return manager.waypoints.waypoints[path[pathIndex]].transform.position;
    }

    public TeacherWaypoint GetWaypoint()
    {
        return manager.waypoints.waypoints[path[pathIndex]];
    }

    void OnDrawGizmosSelected()
    {
        if (manager.waypoints == null || manager.waypoints.waypoints.Contains(null)) return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 start = GizmoGetPos(i);
            Vector3 end = GizmoGetPos(i + 1);

            Gizmos.color = lineColor;

            Gizmos.DrawLine(start, end);
        }

        Gizmos.color = exitColor;
        Gizmos.DrawLine(GizmoGetPos(path.Count - 1), manager.waypoints.waypoints[exitPoint].transform.position);
    }

    public Vector3 GizmoGetPos(int i)
    {
        return manager.waypoints.waypoints[path[i]].transform.position;
    }
}