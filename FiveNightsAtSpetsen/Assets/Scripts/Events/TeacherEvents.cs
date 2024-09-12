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
}