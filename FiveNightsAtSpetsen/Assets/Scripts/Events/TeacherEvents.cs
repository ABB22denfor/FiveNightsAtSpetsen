using System;

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
        OnPlayerMadeSound.Invoke(room);
    }

    public event Action OnPlayerHeard;
    public void PlayerHeard()
    {
        OnPlayerHeard?.Invoke();
    }

    public event Action OnPlayerCaptured;
    public void PlayerCaptured()
    {
        OnPlayerCaptured?.Invoke();
    }

    public event Action<string> OnRoomEntered;
    public void RoomEntered(string roomName)
    {
        OnRoomEntered?.Invoke(roomName);
    }
}
