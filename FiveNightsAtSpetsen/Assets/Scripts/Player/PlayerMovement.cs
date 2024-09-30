using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public Transform orientation;

    public bool immobile;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void OnEnable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid += HidePlayer;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += RevealPlayer;
        EventsManager.Instance.teacherEvents.OnPlayerCaptured += Captured;
    }

    void OnDisable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid -= HidePlayer;
        EventsManager.Instance.playerEvents.OnPlayerRevealed -= RevealPlayer;
        EventsManager.Instance.teacherEvents.OnPlayerCaptured -= Captured;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (immobile) return;

        SetInput();

        SpeedControl();
    }

    void FixedUpdate()
    {
        if (immobile) return;

        MovePlayer();
    }

    void SetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void HidePlayer(GameObject location)
    {
        immobile = true;
    }

    void RevealPlayer(GameObject location)
    {
        immobile = false;
    }

    void Captured() {
        immobile = true;
    }
}