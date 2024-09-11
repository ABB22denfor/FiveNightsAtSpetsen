using UnityEngine;

public class HidingSpot : Interactable
{
    public override void Interact()
    {
        EventsManager.Instance.playerEvents.HidePlayer(gameObject);
    }
}