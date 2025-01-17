using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float maxAcceleration = 1f;

    public Kinematic[] targets;

    float m_radius = 0.5f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        float shortestTime = float.MaxValue;

        Kinematic firstTarget = null;
        float firstMinSeperation = float.MaxValue;
        float firstDistance =  float.MaxValue;
        Vector3 firstRelativePos = Vector3.one;
        Vector3 firstRelativeVel = Vector3.one;

        foreach (Kinematic target in targets)
        {
            Vector3 relativePos = target.transform.position - character.transform.position;
            Vector3 relativeVel = character.linearVelocity - target.linearVelocity;
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

            float distance = relativePos.magnitude;
            float minSeperation = distance - relativeSpeed * timeToCollision;
            if (minSeperation > 2 * m_radius) continue;

            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeperation = minSeperation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        if (firstTarget == null) { return result; }

        float dor = Vector3.Dot(character.linearVelocity.normalized, firstTarget.linearVelocity.normalized);
        if (dor < -0.9f)
            result.linear = new Vector3(character.linearVelocity.z, 0f, character.linearVelocity.x);
        else
            result.linear = -firstTarget.linearVelocity;

        result.linear.Normalize();

        result.linear = firstRelativePos * maxAcceleration;
        result.angular = 0;
        return result;
    }
}
