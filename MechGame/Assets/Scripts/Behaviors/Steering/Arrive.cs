using UnityEngine;
using System.Collections;

public class Arrive : SteeringBehavior {
	public Transform    target;
	public Deceleration deceleration;

	public override Vector3 Force {
		get {
			var to_target = target.position - vehicle.transform.position;

			var dist = to_target.magnitude;

			if (dist > 0) {
				const float decel_tweaker = 0.3f;

				var speed = dist / ((float)deceleration * decel_tweaker);

				speed = Mathf.Min(speed, vehicle.maxSpeed);

				var desired_velocity = to_target * speed / dist;

				return (desired_velocity - vehicle.velocity);
			} else {
				return Vector3.zero;
			}
		}
	}

	public Arrive() {
		deceleration = Deceleration.normal;
	}

	public Arrive(Vector3 t, Deceleration d) {
		target.position = t;
		deceleration = d;
	}
}

public enum Deceleration {
	slow   = 1,
	normal = 2,
	fast   = 3,
}