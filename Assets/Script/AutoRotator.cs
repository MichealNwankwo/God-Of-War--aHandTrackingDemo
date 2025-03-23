using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed = new Vector3(10f, 20f, 30f); // Initial rotation speed

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = initialRotation * Quaternion.Euler(rotationSpeed * Time.time);
    }

    // Method to manually increase speed
    public void IncreaseSpeed(float increment)
    {
        rotationSpeed += new Vector3(increment, increment, increment);
    }
}
