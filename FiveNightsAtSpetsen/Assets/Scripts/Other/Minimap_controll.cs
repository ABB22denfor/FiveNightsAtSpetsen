using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_controll : MonoBehaviour
{
    GameObject image;
    bool IsActive;
    void Start() {
        image = transform.Find("RawImage").gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            image.SetActive(!IsActive);
            IsActive = !IsActive;
            Time.timeScale = IsActive ? 0 : 1;
        }
    }
}
