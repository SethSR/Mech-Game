using UnityEngine;
using System.Collections.Generic;

public class ObstacleAvoidance : SteeringBehavior {
	public float           minDetectionBoxLength;
	public List<Transform> obstacles;

	public override Vector3 Force {
		get {
			var detect_box_length = (vehicle.velocity.magnitude / vehicle.maxSpeed) * minDetectionBoxLength;

			//TODO: TagObstaclesWithinViewRange(vehicle, detect_box_length)

			Transform closest_intersection_obstacle = null;

			var dist_to_closest_ip = float.MaxValue;

			var local_pos_of_closest_obstacle = Vector3.zero;

			foreach (var cur_ob in obstacles) {
				var local_pos = vehicle.transform.position + Quaternion.LookRotation(vehicle.transform.forward) * cur_ob.position;

				if (local_pos.x >= 0) {
					var expanded_radius = cur_ob.GetComponent<SphereCollider>().radius + vehicle.GetComponent<SphereCollider>().radius;

					if (Mathf.Abs(local_pos.y) < expanded_radius) {
						var cx = local_pos.x;
						var cy = local_pos.y;

						var sqrt_part = Mathf.Sqrt(expanded_radius * expanded_radius - cy * cy);

						var ip = cx - sqrt_part;

						if (ip <= 0) {
							ip = cx + sqrt_part;
						}

						if (ip < dist_to_closest_ip) {
							dist_to_closest_ip            = ip;
							closest_intersection_obstacle = cur_ob;
							local_pos_of_closest_obstacle = local_pos;
						}
					}
				}
			}

			var steering_force = Vector3.zero;

			if (closest_intersection_obstacle) {
				var multiplier = 1 + (detect_box_length - local_pos_of_closest_obstacle.x) / detect_box_length;

				steering_force.y = (closest_intersection_obstacle.GetComponent<SphereCollider>().radius - local_pos_of_closest_obstacle.y) * multiplier;

				const float braking_weight = 0.2f;

				steering_force.x = (closest_intersection_obstacle.GetComponent<SphereCollider>().radius - local_pos_of_closest_obstacle.x) * braking_weight;
			}

			return Quaternion.LookRotation(Vector3.forward - vehicle.transform.forward) * steering_force;
		}
	}
}