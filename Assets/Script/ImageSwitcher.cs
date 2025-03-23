using UnityEngine;
using System.Collections;
public class ImageSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects; // Array of objects to cycle through
    [SerializeField]
    private float slideDuration = 0.5f; // Duration of the slide animation

    private int currentIndex = 0;
    private bool isSliding = false;

    void Start()
    {
        UpdateObjects();
    }

    public void MoveRight()
    {
        if (objects.Length == 0 || isSliding) return;
        StartCoroutine(SlideTransition(1));
    }

    public void MoveLeft()
    {
        if (objects.Length == 0 || isSliding) return;
        StartCoroutine(SlideTransition(-1));
    }

    private IEnumerator SlideTransition(int direction)
    {
        isSliding = true;
        GameObject currentObject = objects[currentIndex];
        currentIndex = (currentIndex + direction + objects.Length) % objects.Length;
        GameObject nextObject = objects[currentIndex];

        nextObject.SetActive(true);
        float elapsedTime = 0f;
        Vector3 startPos = currentObject.transform.position;
        Vector3 endPos = startPos + new Vector3(direction * 2f, 0, 0);
        Vector3 nextStartPos = startPos - new Vector3(direction * 2f, 0, 0);
        nextObject.transform.position = nextStartPos;

        while (elapsedTime < slideDuration)
        {
            float t = elapsedTime / slideDuration;
            currentObject.transform.position = Vector3.Lerp(startPos, endPos, t);
            nextObject.transform.position = Vector3.Lerp(nextStartPos, startPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentObject.SetActive(false);
        currentObject.transform.position = startPos;
        nextObject.transform.position = startPos;
        isSliding = false;
    }

    private void UpdateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(i == currentIndex);
        }
    }
}
