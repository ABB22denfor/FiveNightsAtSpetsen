using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Renderer playerRenderer;
    GameObject hidingSpot = null;

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

    void HidePlayer(GameObject location)
    {
        Debug.Log("Hiding player in " + location.name);
        hidingSpot = location;
        playerRenderer.enabled = false;
    }

    void RevealPlayer(GameObject location)
    {
        Debug.Log("Player emerged from " + location.name);
        hidingSpot = null;
        playerRenderer.enabled = true;
    }
}