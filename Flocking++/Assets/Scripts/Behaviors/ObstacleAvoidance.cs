using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    float m_avoidDistance = 1f;
    float m_rayLength = 5f;

    protected override Vector3 getTargetPosition()
    {
        Ray ray = new()
        {
            direction = character.linearVelocity,
            origin = character.transform.position,
        };
        if (Physics.Raycast(ray, out RaycastHit hit, m_rayLength))
        {
            return hit.point - (hit.normal * m_avoidDistance);
        }
        else
        {
            return Vector3.positiveInfinity;
        }
    }
}
