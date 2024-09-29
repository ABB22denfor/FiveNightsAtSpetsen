using UnityEngine;

public class ReplaceMaterialsInScene : MonoBehaviour
{
    public Material originalMaterial;  // The material you want to replace
    public Material newMaterial;       // The material you want to use as a replacement

    void Awake(){
      ReplaceMaterials();
    }

    // Replace all instances of the original material with the new material
    public void ReplaceMaterials()
    {
        // Find all renderers in the scene
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        // Loop through all renderers
        foreach (Renderer renderer in renderers)
        {
            // Get the shared materials array
            Material[] materials = renderer.sharedMaterials;

            // Check each material on the renderer
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == originalMaterial)
                {
                    materials[i] = newMaterial;
                }
            }

            // Apply the modified materials array back to the renderer
            renderer.sharedMaterials = materials;
        }

        Debug.Log("Material replacement completed.");
    }
}

