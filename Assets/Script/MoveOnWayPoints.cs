using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Oculus.Platform.Models;

public class MoveOnWayPoints : MonoBehaviour
{
    public List<GameObject> wayPoints;
    public float speed = 2;
    public float rotationSpeed = 5;
    int index = 0;
    public bool isLoop = true;

    private void Update()
    {
        // Check if there are waypoints to move to
        if (wayPoints.Count == 0)
            return;

        // Get the current destination
        Vector3 destination = wayPoints[index].transform.position;

        // Rotate towards the destination
        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the destination
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;

        // Check if it has reached the current waypoint
        float distance = Vector3.Distance(transform.position, destination);
        if (distance <= 0.05f)
        {
            // Move to the next waypoint
            if (index < wayPoints.Count - 1)
            {
                index++;
            }
            else if (isLoop)
            {
                // Loop back to the first waypoint
                index = 0;
            }
        }
    }
}
