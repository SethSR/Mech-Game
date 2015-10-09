using UnityEngine;
using System.Collections;

public class Flee : SteeringBehavior {
	public Transform target;

	public override Vector3 Force {
		get {
			var desired_velocity = (vehicle.transform.position - target.position).normalized * vehicle.maxSpeed;
			return (desired_velocity - vehicle.velocity);
		}
	}

	public Flee() {}

	public Flee(Vector3 t) {
		target.position = t;
	}
}