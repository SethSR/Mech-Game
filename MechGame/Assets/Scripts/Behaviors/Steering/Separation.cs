using UnityEngine;
using System.Collections.Generic;

public class Separation : SteeringBehavior {
	public List<Mobile> neighbors;

	public override Vector3 Force {
		get {
			var steering_force = Vector3.zero;

			foreach (var neighbor in neighbors) {
				if (neighbor != vehicle) {
					var to_agent = vehicle.transform.position - neighbor.transform.position;
					steering_force += to_agent.normalized / to_agent.magnitude;
				}
			}

			return steering_force;
		}
	}
}