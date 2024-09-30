using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action<HidingSpot> OnPlayerHid;
    public void HidePlayer(HidingSpot spot)
    {
        OnPlayerHid?.Invoke(spot);
    }

    public event Action<HidingSpot> OnPlayerRevealed;
    public void RevealPlayer(HidingSpot spot)
    {
        OnPlayerRevealed?.Invoke(spot);
    }
}