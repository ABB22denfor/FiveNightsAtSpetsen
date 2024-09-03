using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class Teacher : MonoBehaviour
{
    public TeacherPath path;
    public TeacherMovement movement;
    public TeacherRaycasting raycaster;
    public List<(string id, float delay)> delays;

    void Start()
    {
        Init();

        ReachedTarget();
    }

    protected abstract void Init();

    public virtual void ReachedTarget()
    {
        if (raycaster.player == null)
        {
            StartCoroutine(MoveToNext(GetDelay()));
        }
    }

    System.Collections.IEnumerator MoveToNext(float delay)
    {
        yield return new WaitForSeconds(delay);

        path.Next();
        movement.SetTarget(path.GetPos());
    }

    public void PlayerSpotted(Vector3 position)
    {
        movement.SetTarget(position);
    }

    public void PlayerNotSpotted()
    {
        movement.SetTarget(path.GetPos());
    }

    public float GetDelay()
    {
        TeacherWaypoint waypoint = path.GetWaypoint();

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