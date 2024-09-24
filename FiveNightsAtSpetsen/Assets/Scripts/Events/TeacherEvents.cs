using System;
using UnityEngine;

public class TeacherEvents
{
    public event Action<TeacherWaypoint> OnWaypointReached;
    public void WaypointReached(TeacherWaypoint waypoint)
    {
        OnWaypointReached?.Invoke(waypoint);
    }

    public event Action OnPlayerSpotted;
    public void PlayerSpotted()
    {
        OnPlayerSpotted?.Invoke();
    }

    public event Action<TeacherRoomPath> OnPlayerMadeSound;
    public void PlayerMadeSound(TeacherRoomPath room)
    {
        OnPlayerMadeSound?.Invoke(room);
    }

    public event Action<TeacherRoomPath, bool> OnTeacherEnteredRoom;
    public void TeacherEnteredRoom(TeacherRoomPath room, bool isTarget)
    {
        OnTeacherEnteredRoom?.Invoke(room, isTarget);
    }

    public event Action OnPlayerCaptured;
    public void PlayerCaptured()
    {
        OnPlayerCaptured?.Invoke();
    }
}