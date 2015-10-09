using UnityEngine;
using System.Collections;

public class Pursuit : SteeringBehavior {
	public Mobile evader;

	public override Vector3 Force {
		get {
			var to_evader = evader.transform.position - vehicle.transform.position;

			var relative_heading = Vector3.Dot(vehicle.transform.forward, evader.transform.forward);

			if (Vector3.Dot(to_evader, vehicle.transform.forward) > 0 && relative_heading < -0.95f) {
				return new Seek(evader.transform.position).Force;
			} else {
				var look_ahead_time = to_evader.magnitude / (vehicle.maxSpeed + evader.velocity.magnitude);

				return new Seek(evader.transform.position + evader.velocity * look_ahead_time).Force;
			}
		}
	}
}