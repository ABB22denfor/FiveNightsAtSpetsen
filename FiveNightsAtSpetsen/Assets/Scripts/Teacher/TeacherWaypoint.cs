using UnityEngine;

public class TeacherWaypoint : MonoBehaviour
{
    public string id = "";

    void OnDrawGizmos()
    {
        Gizmos.color = (id == "") ? Color.blue : Color.red;

        Gizmos.DrawSphere(transform.position, (id == "") ? 0.5f : 0.75f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = (id == "") ? Color.blue : Color.red;

        Gizmos.DrawSphere(transform.position, ((id == "") ? 0.5f : 0.75f) * 2f);
    }
}