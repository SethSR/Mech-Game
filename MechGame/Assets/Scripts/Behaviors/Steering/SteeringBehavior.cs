using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum Deceleration {
	slow   = 3,
	normal = 2,
	fast   = 1,
}

namespace SteeringBehavior {
	static public class SteeringBehavior {
		static public Vector3 Alignment(this Mobile vehicle, ICollection<Mobile> neighbors) {
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

		static public Vector3 Arrive(this Mobile vehicle, Vector3 target, Deceleration deceleration = Deceleration.normal) {
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

		static public Vector3 Cohesion(this Mobile vehicle, ICollection<Mobile> neighbors) {
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

		static public Vector3 Evade(this Mobile vehicle, Mobile pursuer, float threat_range = 100) {
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
			DebugExtension.DebugWireSphere(vehicle.Position, Color.magenta, threat_range);
			return steering_force;
		}

		static public Vector3 Flee(this Mobile vehicle, Vector3 target) {
			if (vehicle == null) {
				return Vector3.zero;
			}
			var desired_velocity = (vehicle.Position - target).normalized * vehicle.maxSpeed;
			var steering_force = desired_velocity - vehicle.Velocity;
			DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
			return steering_force;
		}

		static public Vector3 FollowPath(this Mobile vehicle, Path path, float waypointSeekDist) {
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

		static public Vector3 Hide(this Mobile vehicle, Mobile hunter, ICollection<Transform> obstacles) {
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

		static public Vector3 Interpose(this Mobile vehicle, Mobile agent_a, Mobile agent_b) {
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

		static public Vector3 ObstacleAvoidance(this Mobile vehicle, ICollection<Transform> obstacles, float min_detection_box_length) {
			if (vehicle == null || obstacles.Count == 0) {
				return Vector3.zero;
			}
			var sc = vehicle.GetComponent<SphereCollider>();

			var half_min_detect_box_length = min_detection_box_length * 0.5f;
			var detect_box_length = half_min_detect_box_length + (vehicle.Speed / vehicle.maxSpeed) * half_min_detect_box_length;
			DebugExtension.DebugCapsule(vehicle.Position + vehicle.Heading * -sc.radius, vehicle.Position + vehicle.Heading * (detect_box_length + sc.radius), Color.magenta, sc.radius);

			var steering_force = Vector3.zero;
			RaycastHit hit_info;
			if (Physics.SphereCast(vehicle.Position, sc.radius, vehicle.Heading, out hit_info, detect_box_length)) {
				var distance   = hit_info.distance;
				var dist_ratio = sc.radius / distance;
				steering_force = hit_info.normal * vehicle.maxSpeed * dist_ratio;
				DebugExtension.DebugArrow(hit_info.point, steering_force, Color.green);
			}
			return steering_force;
		}

		static public Vector3 OffsetPursuit(this Mobile vehicle, Mobile leader, Vector3 offset) {
			if (vehicle == null || leader == null) {
				return Vector3.zero;
			}
			var world_offset_pos = leader.transform.TransformPoint(offset);
			var to_offset = world_offset_pos - vehicle.Position;
			var look_ahead_time = to_offset.magnitude / (vehicle.maxSpeed + leader.Speed);
			var steering_force = Arrive(vehicle, world_offset_pos + leader.Velocity * look_ahead_time, Deceleration.fast);
			DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
			return steering_force;
		}

		static public Vector3 Pursuit(this Mobile vehicle, Mobile evader) {
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

		static public Vector3 Separation(this Mobile vehicle, ICollection<Mobile> neighbors) {
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

		static public Vector3 WallAvoidance(this Mobile vehicle, ICollection<Transform> walls, float wall_feeler_length) {
			if (vehicle == null || walls.Count == 0) {
				return Vector3.zero;
			}
			List<Vector3> feelers          = CreateFeelers(wall_feeler_length);
			Vector3       steering_force   = Vector3.zero;
			Vector3       closest_point    = Vector3.zero;
			float         closest_distance = float.MaxValue;
			foreach (var feeler in feelers) {
				var rot_feeler = vehicle.transform.TransformVector(feeler);
				DebugExtension.DebugArrow(vehicle.Position, rot_feeler, Color.magenta);
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

		static public Vector3 Wander(this Mobile vehicle, float jitter, float radius, float distance, ref Vector3 wander_target) {
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

		static public Vector3 Seek(this Mobile vehicle, Vector3 target) {
			if (vehicle == null) {
				return Vector3.zero;
			}
			var desired_velocity = (target - vehicle.Position).normalized * vehicle.maxSpeed;
			var steering_force = desired_velocity - vehicle.Velocity;
			DebugExtension.DebugArrow(vehicle.Position, steering_force, Color.green);
			return steering_force;
		}

		static public Vector3 Boost(this Mobile vehicle) {
			return Vector3.zero;
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
}