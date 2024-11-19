using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float fovAngle = 90;
    public float fovRange = 5f;
    public Vector2 lookDirection = Vector2.down;
    
    public bool IsTargetInFOV(Transform target)
    {
        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector2.Angle(lookDirection, directionToTarget);

        if (angleToTarget < fovAngle / 2)
        {
            float distance = Vector2.Distance(target.position, transform.position);
            return distance < fovRange;
        }

        return false;
    }
}
