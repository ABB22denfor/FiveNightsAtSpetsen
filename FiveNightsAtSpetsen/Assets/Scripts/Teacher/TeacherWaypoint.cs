using UnityEngine;

public class TeacherWaypoint : MonoBehaviour
{
    public string id = "";

    void OnDrawGizmos()
    {
        Gizmos.color = (id == "") ? Color.blue : Color.red;

        Gizmos.DrawSphere(transform.position, (id == "") ? 0.2f : 0.25f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = (id == "") ? Color.blue : Color.red;

        Gizmos.DrawSphere(transform.position, ((id == "") ? 0.2f : 0.25f) * 2f);
    }
}