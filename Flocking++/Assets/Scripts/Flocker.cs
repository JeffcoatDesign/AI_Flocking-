using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocker : Kinematic
{
    PrioritySteering myMoveType;
    List<Flocker> flock;
    void Start()
    {
        List<BehaviorAndWeight> behaviors = new();
        List<BlendedSteering> groups = new();
        BlendedSteering oldSteering = new();
        flock = new (Flock.instance.flockers.ToArray());
        flock.Remove(this);
        
        Separation separation = new ();
        separation.character = this;
        separation.targets = flock.ToArray();
        BehaviorAndWeight seperationBehavior = new BehaviorAndWeight(separation, 3f);
        behaviors.Add(seperationBehavior);

        VelocityMatch velocityMatch = new VelocityMatch();
        velocityMatch.character = this;
        velocityMatch.target = Flock.instance.flockCenter;
        BehaviorAndWeight velocitymatchBehavior = new BehaviorAndWeight(velocityMatch, 1f);
        behaviors.Add(velocitymatchBehavior);

        LookWhereGoing lookWhereGoing = new LookWhereGoing();
        lookWhereGoing.character = this;
        BehaviorAndWeight lookBehavior = new BehaviorAndWeight (lookWhereGoing, 3f);
        behaviors.Add(lookBehavior);

        Arrive arrive = new Arrive();
        arrive.character = this;
        arrive.target = Flock.instance.flockCenter.gameObject;
        BehaviorAndWeight arriveBehavior = new BehaviorAndWeight(arrive, .4f);
        behaviors.Add(arriveBehavior);

        oldSteering.behaviors = behaviors.ToArray();

        ObstacleAvoidance obstacleAvoidance = new ObstacleAvoidance();
        obstacleAvoidance.character = this;
        obstacleAvoidance.target = Flock.instance.flockCenter.gameObject;
        obstacleAvoidance.flee = true;
        BehaviorAndWeight obstacleAvoidanceBehavior = new BehaviorAndWeight(obstacleAvoidance, 1f);

        BlendedSteering blendedAvoid = new BlendedSteering();
        blendedAvoid.behaviors = new BehaviorAndWeight[1];
        blendedAvoid.behaviors[0] = obstacleAvoidanceBehavior;

        myMoveType = new PrioritySteering();
        groups.Add(blendedAvoid);
        groups.Add(oldSteering);
        myMoveType.groups = groups.ToArray();
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myMoveType.getSteering().angular;
        base.Update();
    }
}
