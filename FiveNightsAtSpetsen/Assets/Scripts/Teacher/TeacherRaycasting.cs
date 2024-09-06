using UnityEngine;

public class TeacherRaycasting : MonoBehaviour
{
    public Teacher teacher;

    public GameObject target;
    public float detectionRange = 10f;
    public LayerMask detectionLayer;

    public GameObject player;

    public float timeSincePlayerSpotted = 0;

    void Update()
    {
        timeSincePlayerSpotted += Time.deltaTime;

        Vector3 directionToTarget = target.transform.position - transform.position;

        if (directionToTarget.magnitude <= detectionRange)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, detectionRange, detectionLayer))
            {
                if (hit.collider.gameObject == target)
                {
                    if (directionToTarget.magnitude < 2f)
                        Debug.Log("Player has been caught");

                    if (timeSincePlayerSpotted >= 0.5f)
                    {
                        timeSincePlayerSpotted = 0f;
                        player = target;
                        teacher.PlayerSpotted(player.transform.position);
                    }
                }
                else
                {
                    if (player != null)
                    {
                        player = null;
                        teacher.PlayerNotSpotted();
                    }
                }
            }
            else
            {
                if (player != null)
                {
                    player = null;
                    teacher.PlayerNotSpotted();
                }
            }
        }
        else
        {
            if (player != null)
            {
                player = null;
                teacher.PlayerNotSpotted();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
