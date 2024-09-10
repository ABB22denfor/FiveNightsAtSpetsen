using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5f;
    public Camera playerCamera;

    Interactable currentInteractable;
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
                    {
                        currentInteractable.HighlightObject(false);
                    }

                    currentInteractable = interactable;
                    currentInteractable.HighlightObject(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.HighlightObject(false);
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.HighlightObject(false);
                currentInteractable = null;
            }
        }
    }
}
