using UnityEngine;
using System.Collections.Generic;

public class TeacherMovement : MonoBehaviour
{
    public Teacher teacher;

    public Vector3 target;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 1f;
    Vector3 startPosition;

    Rigidbody rb;

    public bool chasingPlayer = false;

    public List<Vector3> steps;

    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
    }

    void Update()
    {
        if (steps.Count > 0 && !chasingPlayer)
        {
            if (startPosition == steps[^1]) return;

            Vector3 direction = steps[^1] - transform.position;
            float distanceToTarget = direction.magnitude;

            if (distanceToTarget > 0.5f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (distanceToTarget > 0.1f)
            {
                direction.Normalize();

                rb.velocity = direction * moveSpeed * Time.deltaTime * 100f;
            }
            else
            {
                rb.velocity = Vector3.zero;
                startPosition = steps[^1];

                steps.RemoveAt(steps.Count - 1);
            }
        }
        else
        {
            if (startPosition == target) return;

            Vector3 direction = target - transform.position;
            float distanceToTarget = direction.magnitude;

            if (direction.magnitude > 0.5f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

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
    }

    public void SetTarget(Vector3 target, bool isPlayer = false)
    {
        if (!isPlayer)
        {
            EventsManager.Instance.animationEvents.StartWalking();
        }

        chasingPlayer = isPlayer;

        if (chasingPlayer)
            steps.Add(transform.position);

        this.target = new Vector3(target.x, startPosition.y, target.z);
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target, 0.5f);
    }
}