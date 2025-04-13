using UnityEngine;
using System.Collections;

public class ImageSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private float slideDuration = 0.5f;

    private int currentIndex = 0;
    private bool isSliding = false;

    void Start()
    {
        UpdateObjects();
    }

    public void SetInstantIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= objects.Length) return;

        if (objects[currentIndex] != null)
            objects[currentIndex].SetActive(false);

        currentIndex = newIndex;

        if (objects[currentIndex] != null)
            objects[currentIndex].SetActive(true);
    }

    public void SlideToIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= objects.Length || isSliding || newIndex == currentIndex)
            return;

        int direction = newIndex > currentIndex ? 1 : -1;
        StartCoroutine(SlideTransition(newIndex, direction));
    }

    private IEnumerator SlideTransition(int newIndex, int direction)
    {
        isSliding = true;

        GameObject currentObject = objects[currentIndex];
        GameObject nextObject = objects[newIndex];

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

        currentIndex = newIndex;
        isSliding = false;
    }

    private void UpdateObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
                objects[i].SetActive(i == currentIndex);
        }
    }
}
