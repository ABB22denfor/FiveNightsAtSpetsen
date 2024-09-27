using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public float WalkMultiplier;

    void OnEnable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerSpotted += OnPlayerSpotted;
        EventsManager.Instance.animationEvents.OnStartWalking += StartWalking;
        EventsManager.Instance.animationEvents.OnSetIdle += SetIdle;
    }

    void OnDisable()
    {
        EventsManager.Instance.teacherEvents.OnPlayerSpotted -= OnPlayerSpotted;
        EventsManager.Instance.animationEvents.OnStartWalking -= StartWalking;
        EventsManager.Instance.animationEvents.OnSetIdle -= SetIdle;
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void StartWalking()
    {
        Walking(true);
        Idle(false);
        Running(false);
    }

    void SetIdle()
    {
        Walking(false);
        Idle(true);
        Running(false);
    }

    void OnPlayerSpotted()
    {
        Walking(false);
        Idle(false);
        Running(true);
    }

    void Walking(bool state)
    {
        animator.SetBool("isWalking", state);
    }

    void Idle(bool state)
    {
        animator.SetBool("isIdle", state);
    }

    void Running(bool state)
    {
        animator.SetBool("isRunning", state);
    }

    void SetWalkMultiplier()
    {
        animator.SetFloat("WalkingMultiplier", WalkMultiplier);
    }
}
