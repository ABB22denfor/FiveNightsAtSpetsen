using UnityEngine;
using System.Collections.Generic;

public class TeacherMovement : MonoBehaviour
{
    public Teacher teacher;

    public Vector3 target;

    public float moveSpeed = 5.0f;
    public float investigatingSpeedModifier = 1.25f;
    public float spottedPlayerSpeedModifier = 1.5f;
    public float chasingPlayerSpeedModifier = 2.0f;
    Dictionary<Teacher.TeacherMode, float> speed;
    public float rotationSpeed = 1f;

    Vector3 startPosition;
    Quaternion targetRotation;
    bool immobile = false;

    Rigidbody rb;

    public bool chasingPlayer = false;

    public List<Vector3> steps;

    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

        speed = new() {
            {Teacher.TeacherMode.Standard, moveSpeed},
            {Teacher.TeacherMode.InvestigatingNoise, moveSpeed * investigatingSpeedModifier},
            {Teacher.TeacherMode.SpottedPlayer, moveSpeed * spottedPlayerSpeedModifier},
            {Teacher.TeacherMode.ChasingPlayer, moveSpeed * chasingPlayerSpeedModifier}
        };
    }

    void FixedUpdate()
    {
        if (immobile)
            return;

        if (steps.Count > 0 && !chasingPlayer)
        {
            if (startPosition == steps[^1]) return;

            Vector3 direction = steps[^1] - transform.position;
            float distanceToTarget = direction.magnitude;

            if (distanceToTarget > 5f || targetRotation == null)
                targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (distanceToTarget > 0.25f)
            {
                direction.Normalize();

                rb.velocity = direction * speed[teacher.mode] * Time.deltaTime * 100f;
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

            if (distanceToTarget > 5f || targetRotation == null)
                targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (distanceToTarget > (chasingPlayer ? 5f : 1f))
            {
                direction.Normalize();

                rb.velocity = direction * speed[teacher.mode] * Time.deltaTime * 100f;
            }
            else
            {
                rb.velocity = Vector3.zero;
                startPosition = target;

                teacher.ReachedTarget();

                if (chasingPlayer) {
                    immobile = true;
                    transform.LookAt(teacher.raycaster.player.transform.position);
                    transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                }
            }
        }
    }

    public void SetTarget(Vector3 target, bool isPlayer = false)
    {
        if (!isPlayer)
            EventsManager.Instance.animationEvents.StartWalking();

        chasingPlayer = isPlayer;

        if (chasingPlayer)
            steps.Add(transform.position);

        this.target = new Vector3(target.x, startPosition.y, target.z);
    }

    void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, target);
    }
}