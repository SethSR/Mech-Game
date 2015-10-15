using UnityEngine;
using System.Collections.Generic;

public class WallAvoidance : SteeringBehavior {
	public List<Transform> walls;
	public List<Vector3>   feelers;

	public override Vector3 Force {
		get {
			var dist_to_closest_intersection_point = float.MaxValue;

			var closest_wall_distance = 0f;
			var closest_wall_normal   = Vector3.zero;

			var steering_force = Vector3.zero;
			var point          = Vector3.zero;
			var closest_point  = Vector3.zero;
			var closest_feeler = Vector3.zero;

			foreach (var feeler in feelers) {
				RaycastHit hit;
				if (Physics.Linecast(vehicle.transform.position, feeler, out hit)) {
					if (hit.distance < dist_to_closest_intersection_point) {
						dist_to_closest_intersection_point = hit.distance;
						closest_wall_distance  = hit.distance;
						closest_wall_normal    = hit.normal;
						closest_point          = point;
						closest_feeler         = feeler;
					}
				}
			}

			if (closest_wall_distance >= 0) {
				var overshoot = closest_feeler - closest_point;
				steering_force = closest_wall_normal * overshoot.magnitude;
			}

			return steering_force;
		}
	}
}