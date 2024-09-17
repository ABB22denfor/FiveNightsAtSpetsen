using UnityEngine;
using System.Collections.Generic;

public class TeacherPathManager : MonoBehaviour
{
    public TeacherWaypointsManager waypoints;

    public List<TeacherRoomPath> rooms = new();
    public int currentRoom = 0;

    public bool inRoom = false;
    public List<int> interRoomPath = new() { 0, 1, 2, 3 };
    public int interRoomIndex = 0;
    public int roomDir = 1;

    public TeacherRoomPath target;
    public TeacherRoomPath lastRoom;
    public float roomEntryChance = 0.8f;

    public Color interRoomColor = Color.blue;

    void OnValidate()
    {
        rooms.Clear();

        foreach (Transform child in transform)
            rooms.Add(child.GetComponent<TeacherRoomPath>());
    }

    public void Next()
    {
        if (!inRoom)
        {
            if (target == null)
            {
                foreach (TeacherRoomPath room in rooms)
                {
                    if (room == lastRoom) continue;
                    if (room.exitPoint == interRoomPath[interRoomIndex] && Random.value < roomEntryChance)
                    {
                        inRoom = true;
                        currentRoom = rooms.IndexOf(room);
                        return;
                    }
                }
            }
            else
            {
                if (target.exitPoint == interRoomPath[interRoomIndex])
                {
                    inRoom = true;
                    currentRoom = rooms.IndexOf(target);
                    target = null;
                    return;
                }
            }

            if (roomDir == 1 && interRoomIndex == interRoomPath.Count - 1)
                roomDir = -1;
            else if (roomDir == -1 && interRoomIndex == 0)
                roomDir = 1;

            interRoomIndex += roomDir;
        }
        else
        {
            rooms[currentRoom].Next();
        }
    }

    public void TargetRoom(TeacherRoomPath room)
    {
        target = room;

        int i = interRoomIndex;
        int ti = interRoomPath.IndexOf(room.exitPoint);

        roomDir = ((ti >= i) ? 1 : -1);
    }

    public void RoomFinished(TeacherRoomPath room)
    {
        lastRoom = room;
        inRoom = false;
        interRoomIndex = interRoomPath.IndexOf(room.exitPoint);
    }

    public Vector3 GetPos()
    {
        if (!inRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]].transform.position;
        else
            return rooms[currentRoom].GetPos();
    }

    public TeacherWaypoint GetWaypoint()
    {
        if (!inRoom)
            return waypoints.waypoints[interRoomPath[interRoomIndex]];
        else
            return rooms[currentRoom].GetWaypoint();
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