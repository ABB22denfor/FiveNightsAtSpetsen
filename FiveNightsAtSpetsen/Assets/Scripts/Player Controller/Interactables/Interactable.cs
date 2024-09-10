using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    Renderer objectRenderer;
    Color originalColor;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

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

    public abstract void Interact();
}
