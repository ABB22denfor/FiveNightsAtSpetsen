using UnityEngine;
using System.Collections.Generic;

public class TeacherRoomPath : MonoBehaviour
{
    public string id;
    public Vector3 pos;

    public TeacherPathManager manager;
    public List<int> path = new() { 0, 1, 2, 3, 1 };
    public int exitPoint = 5;
    public int pathIndex = 0;

    public int repetitions = 0;

    public Color lineColor = Color.blue;
    public Color exitColor = Color.red;

    [ContextMenu("Set Pos")]
    void SetPos()
    {
        if (manager.waypoints != null && manager.waypoints.waypoints.Count >= exitPoint)
            pos = manager.waypoints.waypoints[exitPoint].transform.position;
    }

    public void Next()
    {
        if (pathIndex >= path.Count - 1)
        {
            pathIndex = (repetitions == 0 ? -1 : (path[0] == path[^1] ? 0 : -1));
            if (repetitions == 0)
                manager.RoomFinished(this);
            else
                repetitions -= 1;
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