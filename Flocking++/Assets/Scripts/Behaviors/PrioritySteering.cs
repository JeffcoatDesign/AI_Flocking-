using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering : SteeringBehavior
{
    public BlendedSteering[] groups;
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new();

        foreach (var group in groups) {
            result = group.getSteering();
            if (result.linear.magnitude > float.Epsilon || Mathf.Abs(result.angular) > float.Epsilon)
                return result;
        }

        return result;
    }
}
