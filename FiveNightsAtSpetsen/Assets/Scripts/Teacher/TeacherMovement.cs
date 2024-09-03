using UnityEngine;
using System.Collections.Generic;

public class TeacherMovement : MonoBehaviour
{
    public Teacher teacher;

    public Vector3 target;
    public float moveSpeed = 5.0f;
    Vector3 startPosition;

    Rigidbody rb;

    public bool idling = false;

    void Start()
    {
        startPosition = transform.position;

        teacher.path.Next();
        target = teacher.path.GetPos();

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }
    void Update()
    {
        if (idling) return;

        Vector3 direction = target - transform.position;
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget > 0.1f)
        {
            direction.Normalize();

            rb.velocity = direction * moveSpeed * Time.deltaTime * 100f;
        }
        else
        {
            rb.velocity = Vector3.zero;
            startPosition = target;

            teacher.ReachedNewWaypoint();
            idling = true;
        }
    }

    public System.Collections.IEnumerator MoveToNext(float delay)
    {
        yield return new WaitForSeconds(delay);
        idling = false;

        teacher.path.Next();
        target = teacher.path.GetPos();
    }

    void OnDrawGizmos()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target, 0.5f);
    }
}