using UnityEngine;
using System.Collections;

public class Evade : SteeringBehavior {
	public Mobile pursuer;
	public float  threatRange = 100f;

	public override Vector3 Force {
		get {
			var to_pursuer = pursuer.transform.position - vehicle.transform.position;

			if (to_pursuer.sqrMagnitude > threatRange * threatRange) {
				return Vector3.zero;
			}

			var look_ahead_time = to_pursuer.magnitude / (vehicle.maxSpeed + pursuer.velocity.magnitude);

			return new Flee(pursuer.transform.position + pursuer.velocity * look_ahead_time).Force;
		}
	}

	public Evade() {}

	public Evade(Mobile p) {
		pursuer = p;
	}
}