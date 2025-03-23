using UnityEngine;
using System.Collections;

public class SwipeRotate : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // Adjustable speed

    [SerializeField]
    private AudioSource swipeSound; // Slot for swipe sound

    // Rotate Left
    public void RotateLeft()
    {
        StartCoroutine(RotateForDuration(-rotationSpeed, 2f)); // Negative speed for left rotation
    }

    // Rotate Right
    public void RotateRight()
    {
        StartCoroutine(RotateForDuration(rotationSpeed, 2f)); // Positive speed for right rotation
    }

    // Generalized rotation coroutine
    private IEnumerator RotateForDuration(float speed, float duration)
    {
        float elapsedTime = 0f;
        Debug.Log("Rotating");

        // Play sound immediately at the start of rotation
        if (swipeSound != null)
        {
            swipeSound.Play();
        }

        while (elapsedTime < duration)
        {
            // Rotate around the Y-axis in World Space
            transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
