using System;
using UnityEngine;

public class AnimationEvents
{
    public event Action OnStartWalking;
    public void StartWalking()
    {
        OnStartWalking?.Invoke();
    }

    public event Action OnSetIdle;
    public void SetIdle()
    {
        OnSetIdle?.Invoke();
    }
}