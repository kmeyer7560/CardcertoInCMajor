using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Make sure to include this for Light2D

public class FOV : MonoBehaviour
{
    public Transform player; // Reference to the player transform
    public float rayLength = 5f; // Length of the rays
    public int numberOfRays = 10; // Number of rays to cast
    public float coneAngle = 45f; // Angle of the cone in degrees

    private float lastAngle; // Store the last angle when the player was moving

    public Transform hitObject;

    public LayerMask enemyLayer;

    public Light2D light2D; // Reference to the Light2D component

    void Start()
    {

    }

    void Update()
    {
        if (player != null)
        {
            // Get the player's current velocity
            Vector2 movement = player.GetComponent<Rigidbody2D>().velocity;

            // Check if the player is moving
            if (movement.magnitude > 0.1f) // A small threshold to avoid jittering
            {
                // Calculate the angle to rotate towards
                lastAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg; // Update last angle
            }

            // Cast rays in the direction of the last angle
            CastConeRays(movement.magnitude > 0.1f ? lastAngle : lastAngle);

            // Update the light position and angle
            UpdateLight();
        }
    }

    void CastConeRays(float angle)
    {
        // Calculate the angle step based on the number of rays and the cone angle
        float angleStep = coneAngle / numberOfRays;
        float closestDistance = Mathf.Infinity; // Initialize closest distance to infinity
        Transform closestHitObject = null; // Variable to store the closest hit object

        for (int i = 0; i <= numberOfRays; i++)
        {
            // Calculate the current ray angle
            float currentAngle = angle - (coneAngle / 2) + (i * angleStep);

            // Calculate the direction of the ray
            Vector2 direction = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

            // Cast the ray
            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, rayLength, enemyLayer);

            // Draw the ray in the Scene view for debugging
            Debug.DrawRay(player.position, direction * rayLength, Color.red);

            // Check if the ray hit something
            if (hit.collider)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    // Calculate the distance from the player to the hit object
                    float distance = Vector2.Distance(player.position, hit.transform.position);

                    // Check if this hit object is closer than the previously found closest object
                    if (distance < closestDistance)
                    {
                        closestDistance = distance; // Update closest distance
                        closestHitObject = hit.transform; // Update closest hit object
                    }
                }
            }
        }

        // After checking all rays, set hitObject to the closest hit object found
        hitObject = closestHitObject;
    }

    void UpdateLight()
    {
        if (light2D != null)
        {
            // Set the position of the light to the player's position
            
            light2D.transform.position = player.position;
              // Rotate the light to match the direction of the last angle
        light2D.transform.rotation = Quaternion.Euler(0, 0, lastAngle - 90);
        }
    }
}
