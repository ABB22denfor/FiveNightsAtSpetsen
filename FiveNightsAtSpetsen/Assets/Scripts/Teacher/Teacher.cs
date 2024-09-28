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

    public TeacherMode mode = TeacherMode.Standard;

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
        pathManager.teacher = this;
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
        } else {
            EventsManager.Instance.teacherEvents.PlayerCaptured();
        }
    }

    System.Collections.IEnumerator MoveToNext(float delay)
    {
        if (delay > 0)
            EventsManager.Instance.animationEvents.SetIdle();

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

        pathManager.SetAltRoute(true);
        mode = TeacherMode.ChasingPlayer;
        movement.SetTarget(position, true);
    }

    public void PlayerNotSpotted()
    {
        mode = TeacherMode.SpottedPlayer;

        movement.SetTarget(pathManager.GetPos());
    }

    public float GetDelay()
    {
        if (mode != TeacherMode.Standard)
            return 0;

        TeacherWaypoint waypoint = pathManager.GetWaypoint();

        if (delays.ContainsKey(waypoint.id))
            return delays[waypoint.id];
        else
            return 0;
    }

    void PlayerMadeSound(TeacherRoomPath room)
    {
        pathManager.TargetRoom((room, false));

        mode = TeacherMode.InvestigatingNoise;
        pathManager.SetAltRoute(false);
    }

    public void TempAltRouteCompleted() {
        mode = TeacherMode.Standard;
    }

    public enum TeacherMode {
        Standard,
        InvestigatingNoise,
        SpottedPlayer,
        ChasingPlayer
    }
}