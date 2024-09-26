using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation = -180f;

    bool captured = false;

    void OnEnable() {
        EventsManager.Instance.teacherEvents.OnPlayerCaptured += Captured;
    }

    void OnDisable() {
        EventsManager.Instance.teacherEvents.OnPlayerCaptured -= Captured;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (captured) 
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Captured() {
        Vector3 teacherPos = GameObject.Find("Teacher").transform.position;
        transform.LookAt(teacherPos + new Vector3(0, 1, 0));
        captured = true;
    }
}