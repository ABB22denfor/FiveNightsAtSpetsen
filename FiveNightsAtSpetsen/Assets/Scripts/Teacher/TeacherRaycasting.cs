using UnityEngine;

public class TeacherRaycasting : MonoBehaviour
{
    public bool drawVision = true;
    public Teacher teacher;

    public GameObject target;
    public float detectionRange = 10f;
    public LayerMask detectionLayer;
    Renderer targetRenderer;

    public GameObject player;

    public float timeSincePlayerSpotted = 0;

    public bool playerHiding;
    public Vector3 hidingSpot;

    void OnEnable() {
        EventsManager.Instance.playerEvents.OnPlayerHid += PlayerHid;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += PlayerRevealed;
    }

    void OnDisable() {
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

        if (playerHiding && Vector3.Distance(transform.position, hidingSpot) < 10f && Random.value < 0.2f)
            EventsManager.Instance.teacherEvents.PlayerCaptured();

        if (directionToTarget.magnitude <= detectionRange)
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

    void PlayerHid(HidingSpot spot) {
        hidingSpot = spot.transform.position;
        playerHiding = true;
    }

    void PlayerRevealed(HidingSpot spot) {
        playerHiding = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!drawVision) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
