using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    Renderer objectRenderer;
    Color originalColor;
    public Color highlightColor = Color.yellow;

    public float interactionDuration;
    public float interactionProgress;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;

        Init();
    }

    protected virtual void Init() { }

    public virtual void HighlightObject(bool highlight)
    {
        if (highlight && objectRenderer != null)
        {
            objectRenderer.material.color = highlightColor;
        }
        else if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }

    public virtual void Interact()
    {
        interactionProgress = 0f;
    }

    public virtual void Interacting(float dt) {
        interactionProgress += dt;

        if (interactionProgress >= interactionDuration) {
            InteractionCompleted();
        }
    }

    public virtual void StoppedInteracting() {
        interactionProgress = 0f;
    }

    public abstract void InteractionCompleted();
}
