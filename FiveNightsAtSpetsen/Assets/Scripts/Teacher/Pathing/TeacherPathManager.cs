using UnityEngine;
using System.Collections.Generic;

public class TeacherPathManager : MonoBehaviour
{
    public TeacherWaypointsManager waypoints;
    public List<TeacherRoomPath> paths = new();
    public int currentPath = 0;

    public bool interRoom = true;
    public List<int> interRoomPath = new() { 0, 1, 2, 3 };
    public int interRoomIndex = 0;
    public TeacherRoomPath lastRoom;
    public float roomEntryChance = 0.8f;

    public Color interRoomColor = Color.blue;

    void OnValidate()
    {
        paths.Clear();

        foreach (Transform child in transform)
            paths.Add(child.GetComponent<TeacherRoomPath>());
    }

    public void Next()
    {
        if (interRoom)
        {
            foreach (TeacherRoomPath room in paths)
            {
                if (room == lastRoom) continue;
                if (room.exitPoint == interRoomPath[interRoomIndex] && Random.value < roomEntryChance)
                {
                    interRoom = false;
                    currentPath = paths.IndexOf(room);
                    return;
                }
            }

            if (interRoomIndex >= interRoomPath.Count - 1)
                interRoomIndex = -1;

            interRoomIndex++;
        }
        else
        {
            paths[currentPath].Next();
        }
    }

    public void RoomFinished(TeacherRoomPath room)
    {
        lastRoom = room;
        interRoom = true;
        interRoomIndex = interRoomPath.IndexOf(room.exitPoint);
    }

    public Vector3 GetPos()
    {
        if (interRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]].transform.position;
        else
            return paths[currentPath].GetPos();
    }

    public TeacherWaypoint GetWaypoint()
    {
        if (interRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]];
        else
            return paths[currentPath].GetWaypoint();
    }

    void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.waypoints.Contains(null)) return;

        for (int i = 0; i < interRoomPath.Count - 1; i++)
        {
            Vector3 start = GizmoGetPos(i);
            Vector3 end = GizmoGetPos(i + 1);

            Gizmos.color = interRoomColor;

            Gizmos.DrawLine(start, end);
        }
    }

    public Vector3 GizmoGetPos(int i)
    {
        return waypoints.waypoints[interRoomPath[i]].transform.position;
    }
}