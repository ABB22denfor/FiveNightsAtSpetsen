using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Renderer playerRenderer;
    HidingSpot hidingSpot = null;

    void OnEnable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid += HidePlayer;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += RevealPlayer;
    }

    void OnDisable()
    {
        EventsManager.Instance.playerEvents.OnPlayerHid -= HidePlayer;
        EventsManager.Instance.playerEvents.OnPlayerRevealed -= RevealPlayer;
    }

    void Start()
    {
        playerRenderer = transform.Find("PlayerObject").GetComponent<Renderer>();
    }

    void Update()
    {
        if (hidingSpot != null && Input.GetKeyDown(KeyCode.Q))
            EventsManager.Instance.playerEvents.RevealPlayer(hidingSpot);
    }

    void HidePlayer(HidingSpot spot)
    {
        hidingSpot = spot;
        playerRenderer.enabled = false;
    }

    void RevealPlayer(HidingSpot spot)
    {
        hidingSpot = null;
        playerRenderer.enabled = true;
    }
}