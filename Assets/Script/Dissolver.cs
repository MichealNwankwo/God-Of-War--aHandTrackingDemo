using UnityEngine;
using System.Collections;

public class Dissolver : MonoBehaviour
{
    public float dissolveDuration = 2f;
    public float dissolveStrength = 1f;

    public GameObject lightningEffect; // Public GameObject to enable

    public void startDissolver()
    {
        if (lightningEffect != null)
        {
            lightningEffect.SetActive(true); // Enable the lightning effect
            Debug.Log("Lightning effect enabled");
        }

        StartCoroutine(DissolverCoroutine());
    }

    private IEnumerator DissolverCoroutine()
    {
        float elapsedTime = 0f;

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) yield break; // Exit if no renderer is found

        Material[] materials = renderer.materials; // Get all materials

        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(1f, 0f, elapsedTime / dissolveDuration);

            foreach (Material mat in materials) // Apply dissolve effect to all materials
            {
                if (mat.HasProperty("_Dissolve")) // Ensure the material has a dissolve property
                {
                    mat.SetFloat("_Dissolve", dissolveStrength);
                }
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            startDissolver();
            Debug.Log("Dissolving...");
        }
    }
}
