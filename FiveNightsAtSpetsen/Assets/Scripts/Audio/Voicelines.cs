using UnityEngine;

public class Voicelines : MonoBehaviour
{
    void OnEnable()
    {
        EventsManager.Instance.teacherEvents.OnWaypointReached += WaypointReached;
        EventsManager.Instance.teacherEvents.OnPlayerSpotted += PlayerSpotted;
    }
    void OnDisable()
    {
        EventsManager.Instance.teacherEvents.OnWaypointReached -= WaypointReached;
        EventsManager.Instance.teacherEvents.OnPlayerSpotted -= PlayerSpotted;
    }

    void WaypointReached(TeacherWaypoint waypoint)
    {
        Debug.Log("Teacher reached waypoint " + waypoint.id);
    }

    void PlayerSpotted()
    {
        Debug.Log("Teacher spotted player");
    }
}