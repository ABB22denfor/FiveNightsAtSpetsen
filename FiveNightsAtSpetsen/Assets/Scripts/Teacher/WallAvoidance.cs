using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : MonoBehaviour
{
    public Rigidbody rbTeacher;
    public float dir = 90;
    public float forceStrength = 1f;
    public LayerMask layerMask;

    void OnTriggerStay(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & layerMask) != 0)
        {
            Vector3 direction = Quaternion.Euler(0, dir, 0) * transform.forward;
            rbTeacher.AddForce(direction * forceStrength, ForceMode.Acceleration);
        }
    }
}
