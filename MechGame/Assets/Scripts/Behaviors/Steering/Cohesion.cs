using UnityEngine;
using System.Collections.Generic;

public class Cohesion : SteeringBehavior {
	public Mobile       targetAgent;
	public List<Mobile> neighbors;

	public override Vector3 Force {
		get {
			var center_of_mass = Vector3.zero;
			var steering_force = Vector3.zero;

			var neighbor_count = 0;

			foreach (var neighbor in neighbors) {
				if (neighbor != vehicle) {
					center_of_mass += neighbor.transform.position;
					++neighbor_count;
				}
			}

			if (neighbor_count > 0) {
				center_of_mass /= neighbor_count;
				steering_force  = new Seek(center_of_mass).Force;
			}

			return steering_force.normalized;
		}
	}
}