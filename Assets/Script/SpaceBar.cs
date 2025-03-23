using UnityEngine;
using System.Collections;
public class SpaceBar : MonoBehaviour
{
    public float dissolveDuration = 2;
    public float dissolveStrength = 1;


    public void startDissolver()
    {
        StartCoroutine(dissolver());
    }

    public IEnumerator dissolver()
    {
        float elapsedTime = 0;

        Material dissolveMaterial = GetComponent<Renderer>().material;
        while (elapsedTime < dissolveDuration)
        {
            elapsedTime += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(1, 0, elapsedTime / dissolveDuration);
            dissolveMaterial.SetFloat("_Dissolve", dissolveStrength);
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Dissolving");
            startDissolver();
           
        }
    }
}
