using UnityEngine;
using System.Collections.Generic;

public class WallAvoidance : SteeringBehavior {
	public List<Transform> walls;

	public override Vector3 Force {
		get {
			var dist_to_this_intersection_point = 0f;
			var dist_to_closest_intersection_point = float.MaxValue;

			var closest_wall = -1;

			var steering_force = Vector3.zero;
			var point          = Vector3.zero;
			var closest_point  = Vector3.zero;
		}
	}
}