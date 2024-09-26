using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5f;
    public Camera playerCamera;
    public Slider slider;

    Interactable currentInteractable;
    bool interacting = false;
    GameObject playerObject;

    void Start()
    {
        playerObject = transform.Find("PlayerObject").gameObject;
    }

    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    if (currentInteractable != null)
                        currentInteractable.HighlightObject(false);

                    currentInteractable = interactable;
                    interacting = false;
                    currentInteractable.HighlightObject(true);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    if (!interacting) 
                    {
                        currentInteractable.Interact();
                        InitSlider();
                        interacting = true;
                    }
                    currentInteractable.Interacting(Time.deltaTime);
                } 
                else 
                {
                    currentInteractable.StoppedInteracting();
                    interacting = false;
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.HighlightObject(false);
                    currentInteractable.StoppedInteracting();
                    interacting = false;
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.HighlightObject(false);
                currentInteractable.StoppedInteracting();
                interacting = false;
                currentInteractable = null;
            }
        }

        UpdateSlider();
    }

    void InitSlider() {
        if (currentInteractable.interactionDuration == 0)
            return;
            
        slider.gameObject.SetActive(true);
        slider.maxValue = currentInteractable.interactionDuration;
        slider.value = 0;
    }

    void UpdateSlider() {
        if (!interacting || currentInteractable.interactionProgress >= currentInteractable.interactionDuration) {
            slider.gameObject.SetActive(false);
            return;
        }

        slider.value = currentInteractable.interactionProgress;
    }
}
