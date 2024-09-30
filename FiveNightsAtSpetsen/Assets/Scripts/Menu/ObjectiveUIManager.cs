using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveUIManager : MonoBehaviour
{
    private GameObject Canvas;
    private GameObject TipsObject;
    private GameObject ObjectiveObject;

    void Start()
    {
        Transform Canvas = GameObject.Find("Canvas").transform;
        TipsObject = Canvas.Find("Tips").gameObject;
        ObjectiveObject = Canvas.Find("Objectives").gameObject;
    }
}
