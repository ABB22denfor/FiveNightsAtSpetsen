using UnityEngine;

public class WaypointGizmo : MonoBehaviour
{
    public TeacherWaypoint waypointScript;
    public Color gizmoColor = Color.blue;
    public float radius = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = (waypointScript.id == "") ? gizmoColor : Color.red;

        Gizmos.DrawSphere(transform.position, (waypointScript.id == "") ? radius : radius * 2);
    }
}
