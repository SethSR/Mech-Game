using UnityEngine;
using System.Collections;

public class Interpose : SteeringBehavior {
	public Mobile agentA, agentB;

	public override Vector3 Force {
		get {
			var mid_point = (agentA.transform.position + agentB.transform.position) / 2;

			var time_to_reach_mid_point = (vehicle.transform.position - mid_point).magnitude / vehicle.maxSpeed;

			var posA = agentA.transform.position + agentA.velocity * time_to_reach_mid_point;
			var posB = agentB.transform.position + agentB.velocity * time_to_reach_mid_point;

			mid_point = (posA + posB) / 2;

			return new Arrive(mid_point, Deceleration.fast).Force;
		}
	}
}