using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Wander : SteeringBehavior {
	public float  wanderJitter;
	public float  wanderRadius;
	public float  wanderDistance;

	Vector3 wanderTarget;

	public override Vector3 Force {
		get {
			var jitter_this_time_slice = wanderJitter * Time.deltaTime;
			wanderTarget += new Vector3((Random.value * 2 - 1) * jitter_this_time_slice,
			                            (Random.value * 2 - 1) * jitter_this_time_slice,
			                            (Random.value * 2 - 1) * jitter_this_time_slice);
			wanderTarget.Normalize();
			wanderTarget *= wanderRadius;
			var target = wanderTarget + vehicle.transform.forward * wanderDistance;

			// Debug rendering
			// DebugExtension.DebugWireSphere(
			// 	vehicle.transform.position + vehicle.transform.forward * wanderDistance * GetComponent<SphereCollider>().radius,
			// 	Color.green,
			// 	wanderRadius * GetComponent<SphereCollider>().radius);
			// DebugExtension.DebugArrow(vehicle.transform.position, target, Color.red);
			// DebugExtension.DebugArrow(Vector3.zero, vehicle.transform.forward, Color.cyan);
			// DebugExtension.DebugArrow(Vector3.zero, wanderTarget + vehicle.transform.forward * wanderDistance);

			return target;
		}
	}

	Vector3 PointToWorldSpace(Vector3 point,
	                          Vector3 agent_heading,
	                          Vector3 agent_position) {
		return agent_position + Quaternion.LookRotation(agent_heading) * point;
	}
}