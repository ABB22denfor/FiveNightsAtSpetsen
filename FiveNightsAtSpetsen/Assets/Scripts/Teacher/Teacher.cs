using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Teacher : MonoBehaviour
{
    public TeacherPathManager pathManager;
    public TeacherMovement movement;
    public TeacherRaycasting raycaster;
    public TextAsset dataJson;
    Dictionary<string, float> delays;

    void OnEnable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerMadeSound += PlayerMadeSound;
    }

    void OnDisable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerMadeSound -= PlayerMadeSound;
    }

    void Start()
    {
        // pathManager.Next();
        movement.SetTarget(pathManager.GetPos());

        TeacherDataParser dataParser = new(dataJson);
        delays = dataParser.GetDelays();
        pathManager.SetRoute(dataParser.GetRoute());
    }

    public void ReachedTarget()
    {
        if (raycaster.player == null)
        {
            EventsManager.Instance.teacherEvents.WaypointReached(pathManager.GetWaypoint());
            StartCoroutine(MoveToNext(GetDelay()));
        }
    }

    System.Collections.IEnumerator MoveToNext(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (raycaster.player == null)
        {
            pathManager.Next();
            movement.SetTarget(pathManager.GetPos());
        }
    }

    public void PlayerSpotted(Vector3 position)
    {
        if (!movement.chasingPlayer)
            EventsManager.Instance.teacherEvents.PlayerSpotted();
        movement.SetTarget(position, true);
    }

    public void PlayerNotSpotted()
    {
        movement.SetTarget(pathManager.GetPos());
    }

    public float GetDelay()
    {
        TeacherWaypoint waypoint = pathManager.GetWaypoint();

        if (delays.ContainsKey(waypoint.id))
            return delays[waypoint.id];
        else
            return 0;
    }

    void PlayerMadeSound(TeacherRoomPath room)
    {
        Debug.Log("Teacher heard a sound coming from " + room.id);
        pathManager.TargetRoom(room);
    }
}