using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation = -180f;

    bool immobile = false;

    bool hidden = false;
    Vector3 oldPos;
    float initialHiddenYRotation;
    float hiddenYRotation;

    void OnEnable() {
        EventsManager.Instance.playerEvents.OnPlayerHid += Hide;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += Emerge;
        EventsManager.Instance.teacherEvents.OnPlayerCaptured += Captured;
    }

    void OnDisable() {
        EventsManager.Instance.playerEvents.OnPlayerHid -= Hide;
        EventsManager.Instance.playerEvents.OnPlayerRevealed -= Emerge;
        EventsManager.Instance.teacherEvents.OnPlayerCaptured -= Captured;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (immobile) 
            return;

        if (!hidden) {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        } 
        else
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensY / 2;

            hiddenYRotation += mouseX;
            hiddenYRotation = Mathf.Clamp(hiddenYRotation, 
                                          initialHiddenYRotation - 60f, 
                                          initialHiddenYRotation + 60f);

            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, hiddenYRotation, 0);
        }
    }

    void Hide(GameObject go) {
        oldPos = transform.position;
        transform.position = go.transform.position;
        hidden = true;
        transform.LookAt(go.transform.Find("CameraDirection").position);
        initialHiddenYRotation = transform.eulerAngles.y;
        hiddenYRotation = transform.eulerAngles.y;
    }

    void Emerge(GameObject go) {
        transform.position = oldPos;
        hidden = false;
    }

    void Captured() {
        Vector3 teacherPos = GameObject.Find("Teacher").transform.position;
        transform.LookAt(teacherPos + new Vector3(0, 1, 0));
        immobile = true;
    }
}