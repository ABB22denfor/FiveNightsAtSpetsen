using UnityEngine;
using System.Collections.Generic;

public class TeacherMovement : MonoBehaviour
{
    public List<Vector3> targets = new();
    public Vector3 currentTarget;
    public float moveSpeed = 5.0f;
    int posMod = -1;

    Vector3 startPosition;

    Rigidbody rb;


    void Start()
    {
        startPosition = transform.position;
        currentTarget = targets[0];

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }
    void Update()
    {
        Vector3 direction = currentTarget - transform.position;
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget > 0.1f)
        {
            direction.Normalize();

            rb.velocity = direction * moveSpeed * Time.deltaTime * 100f;
        }
        else
        {
            rb.velocity = Vector3.zero;

            startPosition = currentTarget;

            int index = targets.IndexOf(currentTarget);
            if (index == targets.Count - 1 || index == 0)
                posMod *= -1;

            currentTarget = targets[index + posMod];
        }
    }

}