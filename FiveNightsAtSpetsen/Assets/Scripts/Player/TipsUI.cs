using UnityEngine;
using TMPro;

public class TipsUI : MonoBehaviour 
{
    RectTransform rect;
    TextMeshProUGUI tmp;

    void OnEnable() 
    {
        EventsManager.Instance.playerEvents.OnPlayerHid += PlayerHid;
        EventsManager.Instance.playerEvents.OnPlayerRevealed += PlayerRevealed;
    }

    void OnDisable() 
    {
        EventsManager.Instance.playerEvents.OnPlayerHid -= PlayerHid;
        EventsManager.Instance.playerEvents.OnPlayerRevealed -= PlayerRevealed;
    }

    void Start() 
    {
        tmp = transform.Find("Info").GetComponent<TextMeshProUGUI>();
        rect = transform as RectTransform;
        
        tmp.text = "Använd 'E' för att interagera";
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tmp.preferredHeight + 50 + 20);
    }

    void PlayerHid(HidingSpot spot) 
    {
        tmp.text = "Använd 'Q' för att lämna gömstället";
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tmp.preferredHeight + 50 + 20);
    }

    void PlayerRevealed(HidingSpot spot) 
    {
        tmp.text = "Använd 'E' för att interagera";
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, tmp.preferredHeight + 50 + 20);
    }
}