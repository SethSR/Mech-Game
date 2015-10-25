using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum Deceleration {
	slow   = 3,
	normal = 2,
	fast   = 1,
}

static public class SteeringBehavior {
	static public Vector3 Alignment(Mobile vehicle, ICollection<Mobile> neighbors) {
		if (vehicle == null || neighbors.Count == 0) {
			return Vector3.zero;
		}
		var average_heading = neighbors
			.Select(m => m.Heading)
			.Aggregate((a,b) => a + b);
		average_heading /= neighbors.Count;
		average_heading -= vehicle.Heading;
		DebugExtension.DebugArrow(vehicle.Position, average_heading, Color.green);
		return average_heading;
	}

	static public Vector3 Arrive(Mobile vehicle, Vector3 target, Deceleration deceleration = Deceleration.normal) {
		if (vehicle == null) {
			return Vector3.zero;
		}
		var to_target = target - vehicle.Position;
		var dist = to_target.magnitude;

		var steering_force = Vector3.zero;
		if (dist > 0) {
			const float decel_tweaker = 1.2f;
			var speed = dist / ((float)deceleration * decel_tweaker);
			speed = Mathf.Min(speed, vehicle.maxSpeed);
			var desired_velocity = to_target * speed / dist;
			steering_force = desired_velocity - vehicle.Velocity;
		}
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Cohesion(Mobile vehicle, ICollection<Mobile> neighbors) {
		if (vehicle == null || neighbors.Count == 0) {
			return Vector3.zero;
		}
		var center_of_mass = neighbors
			.Select(m => m.Position)
			.Aggregate((a,b) => a + b);
		center_of_mass /= neighbors.Count;
		var steering_force = Seek(vehicle, center_of_mass).normalized;
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Evade(Mobile vehicle, Mobile pursuer, float threat_range = 100) {
		if (vehicle == null || pursuer == null) {
			return Vector3.zero;
		}
		var to_pursuer = pursuer.Position - vehicle.Position;
		if (to_pursuer.sqrMagnitude > threat_range * threat_range) {
			return Vector3.zero;
		}
		var look_ahead_time = to_pursuer.magnitude / (vehicle.maxSpeed + pursuer.Speed);
		var steering_force = Flee(vehicle, pursuer.Position + pursuer.Velocity * look_ahead_time);
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		DebugExtension.DebugWireSphere(vehicle.Position, Color.red, threat_range);
		return steering_force;
	}

	static public Vector3 Flee(Mobile vehicle, Vector3 target) {
		if (vehicle == null) {
			return Vector3.zero;
		}
		var desired_velocity = (vehicle.Position - target).normalized * vehicle.maxSpeed;
		var steering_force = desired_velocity - vehicle.Velocity;
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 FollowPath(Mobile vehicle, Path path, float waypointSeekDist) {
		if (vehicle == null || path == null || path.IsEmpty) {
			return Vector3.zero;
		}
		if ((path.CurrentWaypoint - vehicle.Position).sqrMagnitude < waypointSeekDist * waypointSeekDist) {
			path.SetNextWaypoint();
		}
		var steering_force = Vector3.zero;
		if (!path.Finished) {
			steering_force = Seek(vehicle, path.CurrentWaypoint);
		} else {
			steering_force = Arrive(vehicle, path.CurrentWaypoint);
		}
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		DebugExtension.DebugWireSphere(vehicle.Position, Color.red, waypointSeekDist);
		return steering_force;
	}

	static public Vector3 Hide(Mobile vehicle, Mobile hunter, ICollection<Transform> obstacles) {
		if (vehicle == null || hunter == null) {
			return Vector3.zero;
		}
		var dist_to_closest = float.MaxValue;
		var best_hiding_spot = Vector3.zero;

		foreach (Transform cur_ob in obstacles) {
			var hiding_spot = GetHidingPosition(cur_ob.position,
			                                    cur_ob.GetComponent<SphereCollider>().radius,
			                                    hunter.Position);
			DebugExtension.DebugPoint(hiding_spot, Color.black);

			var dist = (hiding_spot - vehicle.Position).magnitude;
			if (dist < dist_to_closest) {
				dist_to_closest  = dist;
				best_hiding_spot = hiding_spot;
			}
		}
		var steering_force = Vector3.zero;
		if (dist_to_closest == float.MaxValue) {
			steering_force = Evade(vehicle, hunter);
		} else {
			steering_force = Arrive(vehicle, best_hiding_spot, Deceleration.fast);
		}
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Interpose(Mobile vehicle, Mobile agent_a, Mobile agent_b) {
		if (vehicle == null || agent_a == null || agent_b == null) {
			return Vector3.zero;
		}
		var mid_point = (agent_a.Position + agent_b.Position) / 2;
		var time_to_reach_mid_point = (vehicle.Position - mid_point).magnitude / vehicle.maxSpeed;
		var pos_a = agent_a.Position + agent_a.Velocity * time_to_reach_mid_point;
		var pos_b = agent_b.Position + agent_b.Velocity * time_to_reach_mid_point;
		mid_point = (pos_a + pos_b) / 2;
		DebugExtension.DebugPoint(mid_point, Color.green);
		var steering_force = Arrive(vehicle, mid_point, Deceleration.fast);
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	//FIXME(seth): Fix this!! It's doing something weird
	static public Vector3 ObstacleAvoidance(Mobile vehicle, ICollection<Transform> obstacles, float min_detection_box_length) {
		if (vehicle == null || obstacles.Count == 0) {
			return Vector3.zero;
		}
		//the detection box length is proportional to the agent's Velocity
		var detect_box_length = min_detection_box_length + (vehicle.Speed / vehicle.maxSpeed) * min_detection_box_length;
		DebugExtension.DebugCylinder(vehicle.Position, vehicle.Position + vehicle.Heading * detect_box_length, Color.red, vehicle.GetComponent<SphereCollider>().radius);

		//this will keep track of the closest intersecting obstacle (CIB)
		Transform closest_intersecting_obstacle = null;
	 
		//this will be used to track the distance to the CIB
		float dist_to_closest_ip = float.MaxValue;

		//this will record the transformed local coordinates of the CIB
		var local_pos_of_closest_obstacle = Vector3.zero;

		foreach (var cur_ob in obstacles) {
			//calculate this obstacle's position in local space
			var local_pos = Quaternion.FromToRotation(vehicle.Heading, Vector3.forward) * (cur_ob.position - vehicle.Position);

			//if the local position has a negative x value then it must lay
			//behind the agent. (in which case it can be ignored)
			if (local_pos.x >= 0) {
				//if the distance from the x axis to the object's position is less
				//than its radius + half the width of the detection box then there
				//is a potential intersection.
				var expanded_radius = cur_ob.GetComponent<SphereCollider>().radius + vehicle.GetComponent<SphereCollider>().radius;

				if (Mathf.Abs(local_pos.y) < expanded_radius) {
					//now to do a line/circle intersection test. The center of the 
					//circle is represented by (cx, cy). The intersection points are 
					//given by the formula x = cX +/-sqrt(r^2-cy^2) for y=0. 
					//We only need to look at the smallest positive value of x because
					//that will be the closest point of intersection.
					var cx = local_pos.x;
					var cy = local_pos.y;

					//we only need to calculate the sqrt part of the above equation once
					var sqrt_part = Mathf.Sqrt(expanded_radius * expanded_radius - cy * cy);

					var ip = cx - sqrt_part;

					if (ip <= 0.0) {
						ip = cx + sqrt_part;
					}

					//test to see if this is the closest so far. If it is keep a
					//record of the obstacle and its local coordinates
					if (ip < dist_to_closest_ip) {
						dist_to_closest_ip = ip;
						closest_intersecting_obstacle = cur_ob;
						local_pos_of_closest_obstacle = local_pos;
					}
				}
			}
		}

		//if we have found an intersecting obstacle, calculate a steering 
		//force away from it
		var steering_force = Vector3.zero;

		if (closest_intersecting_obstacle != null) {
			//the closer the agent is to an object, the stronger the 
			//steering force should be
			var multiplier = 1.0f + (detect_box_length - local_pos_of_closest_obstacle.x) / detect_box_length;

			//calculate the lateral force
			steering_force.y = (closest_intersecting_obstacle.GetComponent<SphereCollider>().radius - local_pos_of_closest_obstacle.y)  * multiplier;

			//apply a braking force proportional to the obstacles distance from
			//the vehicle. 
			const float braking_weight = 0.2f;

			steering_force.x = (closest_intersecting_obstacle.GetComponent<SphereCollider>().radius - local_pos_of_closest_obstacle.x) * braking_weight;
		}

		//finally, convert the steering vector from local to world space
		var force = vehicle.Position + Quaternion.LookRotation(Vector3.forward - vehicle.Heading) * steering_force;
		DebugExtension.DebugArrow(vehicle.Position, force - vehicle.Position, Color.green);
		// return force;
		return Vector3.zero;
	}

	static public Vector3 OffsetPursuit(Mobile vehicle, Mobile leader, Vector3 offset) {
		if (vehicle == null || leader == null) {
			return Vector3.zero;
		}
		var world_offset_pos = leader.Position + Quaternion.LookRotation(leader.Heading) * offset;
		var to_offset = world_offset_pos - vehicle.Position;
		var look_ahead_time = to_offset.magnitude / (vehicle.maxSpeed + leader.Speed);
		var steering_force = Arrive(vehicle, world_offset_pos + leader.Velocity * look_ahead_time, Deceleration.fast);
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Pursuit(Mobile vehicle, Mobile evader) {
		if (vehicle == null || evader == null) {
			return Vector3.zero;
		}
		var to_evader = evader.Position - vehicle.Position;
		var relative_heading = Vector3.Dot(vehicle.Heading, evader.Heading);

		Vector3 steering_force = Vector3.zero;
		if (Vector3.Dot(to_evader, vehicle.Heading) > 0 && relative_heading < -0.95f) {
			steering_force = Seek(vehicle, evader.Position);
		} else {
			var look_ahead_time = to_evader.magnitude / (vehicle.maxSpeed + evader.Speed);
			steering_force = Seek(vehicle, evader.Position + evader.Velocity * look_ahead_time);
		}
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Separation(Mobile vehicle, ICollection<Mobile> neighbors) {
		if (vehicle == null || neighbors.Count == 0) {
			return Vector3.zero;
		}
		var steering_force = neighbors
			.Select(m => vehicle.Position - m.Position)
			.Select(v => v.normalized / v.magnitude)
			.Aggregate((a,b) => a + b);
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 WallAvoidance(Mobile vehicle, ICollection<Transform> walls, float wall_feeler_length) {
		if (vehicle == null || walls.Count == 0) {
			return Vector3.zero;
		}
		List<Vector3> feelers          = CreateFeelers(wall_feeler_length);
		Vector3       steering_force   = Vector3.zero;
		Vector3       closest_point    = Vector3.zero;
		float         closest_distance = float.MaxValue;
		foreach (var feeler in feelers) {
			var rot_feeler = vehicle.transform.TransformVector(feeler);
			DebugExtension.DebugArrow(vehicle.Position, rot_feeler, Color.gray);
			RaycastHit hit_info;
			if (Physics.Linecast(vehicle.Position, rot_feeler + vehicle.Position, out hit_info)) {
				DebugExtension.DebugPoint(hit_info.point, Color.red);
				if (hit_info.distance < closest_distance) {
					closest_distance = hit_info.distance;
					closest_point    = hit_info.point;
					steering_force   = hit_info.normal * (rot_feeler - closest_point).magnitude;
				}
			}
		}
		DebugExtension.DebugArrow(closest_point, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Wander(Mobile vehicle, float jitter, float radius, float distance, ref Vector3 wander_target) {
		if (vehicle == null) {
			return Vector3.zero;
		}
		var jitter_this_time_slice = jitter * Time.deltaTime;
		wander_target += new Vector3((Random.value * 2 - 1) * jitter_this_time_slice,
		                             (Random.value * 2 - 1) * jitter_this_time_slice,
		                             (Random.value * 2 - 1) * jitter_this_time_slice);
		wander_target.Normalize();
		wander_target *= radius;
		var steering_force = wander_target + vehicle.Heading * distance;
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}

	static public Vector3 Seek(Mobile vehicle, Vector3 target) {
		if (vehicle == null) {
			return Vector3.zero;
		}
		var desired_velocity = (target - vehicle.Position).normalized * vehicle.maxSpeed;
		var steering_force = desired_velocity - vehicle.Velocity;
		DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
		return steering_force;
	}


	static Vector3 GetHidingPosition(Vector3 pos_ob, float radius_ob, Vector3 pos_hunter) {
		const float distance_from_boundary = 3f;
		var dist_away = radius_ob + distance_from_boundary;
		var to_ob = (pos_ob - pos_hunter).normalized;
		return (to_ob * dist_away) + pos_ob;
	}

	static List<Vector3> CreateFeelers(float wall_feeler_length) {
		List<Vector3> feelers = new List<Vector3>(5);
		feelers.Add(Quaternion.Euler(  0,  0, 0) * Vector3.forward * wall_feeler_length);
		feelers.Add(Quaternion.Euler( 45,  0, 0) * Vector3.forward * wall_feeler_length * 0.5f);
		feelers.Add(Quaternion.Euler(-45,  0, 0) * Vector3.forward * wall_feeler_length * 0.5f);
		feelers.Add(Quaternion.Euler(  0, 45, 0) * Vector3.forward * wall_feeler_length * 0.5f);
		feelers.Add(Quaternion.Euler(  0,-45, 0) * Vector3.forward * wall_feeler_length * 0.5f);
		return feelers;
	}
}