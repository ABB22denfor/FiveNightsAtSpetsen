using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Teacher : MonoBehaviour
{
    public TeacherPathManager pathManager;
    public TeacherMovement movement;
    public TeacherRaycasting raycaster;
    public List<(string id, float delay)> delays;

    void Start()
    {
        Init();

        pathManager.Next();
        movement.SetTarget(pathManager.GetPos());
    }

    protected abstract void Init();

    public virtual void ReachedTarget()
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

        if (waypoint.id == "")
        {
            return 0;
        }
        else
        {
            foreach ((string id, float delay) item in delays)
            {
                if (item.id == waypoint.id)
                    return item.delay;
            }

            return 0;
        }
    }
}