using UnityEngine;

public class HidingSpot : Interactable
{
    public Vector3 offset = new(0, 0, 0);
    public float bounds = 60f;

    public override void InteractionCompleted() {
        EventsManager.Instance.playerEvents.HidePlayer(this);
    }
}