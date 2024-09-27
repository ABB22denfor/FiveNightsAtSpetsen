using System.Collections;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }
    public PlayerEvents playerEvents;
    public TeacherEvents teacherEvents;
    public TaskEvents taskEvents;
    public AnimationEvents animationEvents;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Events Manager in the scene.");
        }
        Instance = this;

        playerEvents = new();
        teacherEvents = new();
        taskEvents = new();
        animationEvents = new();
    }
}