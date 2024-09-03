using UnityEngine;
using System.Collections.Generic;

public class TeacherMovement : MonoBehaviour
{
    public TeacherPath path;

    public Vector3 target;
    public float moveSpeed = 5.0f;
    Vector3 startPosition;

    Rigidbody rb;

    void Start()
    {
        startPosition = transform.position;

        path.Next();
        target = path.GetPos();

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }
    void Update()
    {
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

            path.Next();
            target = path.GetPos();
        }
    }

}