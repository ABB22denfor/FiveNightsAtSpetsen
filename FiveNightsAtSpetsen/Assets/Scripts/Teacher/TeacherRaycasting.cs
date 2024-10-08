using UnityEngine;

public class TeacherRaycasting : MonoBehaviour
{
    public bool drawVision = true;
    public Teacher teacher;

    public GameObject target;
    public float detectionRange = 10f;
    public float detectionArc = 180f;
    public LayerMask detectionLayer;
    Renderer targetRenderer;

    public GameObject player { get; private set; }

    public float timeSincePlayerSpotted = 0;

    public bool playerHiding;
    public Vector3 hidingSpot;

    void OnEnable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid += PlayerHid;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += PlayerRevealed;
    }

    void OnDisable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid -= PlayerHid;
        EventsManager.Instance.playerEvents.OnPlayerRevealed -= PlayerRevealed;
    }

    void Start()
    {
        targetRenderer = target.GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        timeSincePlayerSpotted += Time.deltaTime;

        Vector3 directionToTarget = target.transform.position - transform.position;


        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget <= detectionArc / 2f && directionToTarget.magnitude <= detectionRange)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, detectionRange, detectionLayer))
            {
                if (hit.collider.gameObject == target && targetRenderer.enabled)
                {
                    if (timeSincePlayerSpotted >= 0.5f)
                    {
                        timeSincePlayerSpotted = 0f;
                        player = target;
                        teacher.PlayerSpotted(player.transform.position);
                    }

                    if (playerHiding && Vector3.Distance(transform.position, hidingSpot) < 10f)
                        EventsManager.Instance.teacherEvents.PlayerCaptured();

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

    void PlayerHid(HidingSpot spot)
    {
        hidingSpot = spot.transform.position;
        playerHiding = true;
    }

    void PlayerRevealed(HidingSpot spot)
    {
        playerHiding = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!drawVision) return;

        Gizmos.color = Color.yellow;

        Vector3 forward = transform.forward;

        Quaternion leftRayRotation = Quaternion.Euler(0, -detectionArc / 2f, 0);
        Vector3 leftRayDirection = leftRayRotation * forward;

        Quaternion rightRayRotation = Quaternion.Euler(0, detectionArc / 2f, 0);
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * detectionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * detectionRange);
    }
}
