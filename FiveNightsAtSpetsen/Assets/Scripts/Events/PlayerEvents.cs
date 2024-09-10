using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action<GameObject> OnPlayerHid;
    public void HidePlayer(GameObject gameObject)
    {
        OnPlayerHid?.Invoke(gameObject);
    }

    public event Action<GameObject> OnPlayerRevealed;
    public void RevealPlayer(GameObject gameObject)
    {
        OnPlayerRevealed?.Invoke(gameObject);
    }
}