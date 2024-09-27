using UnityEngine;

public class HidingSpot : Interactable
{
    public override void InteractionCompleted() {
        EventsManager.Instance.playerEvents.HidePlayer(gameObject);
    }
}