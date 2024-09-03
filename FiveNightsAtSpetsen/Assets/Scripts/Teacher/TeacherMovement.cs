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
    public bool chasingPlayer = false;

    void Start()
    {
        startPosition = transform.position;

        target = transform.position;

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }

    void Update()
    {
        if (startPosition == target) return;

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

            teacher.ReachedTarget();
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target, 0.5f);
    }
}