using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class Wander : SteeringBehavior {
	public float wanderJitter;
	public float wanderRadius;
	public float wanderDistance;

	Vector3 wanderTarget;
	
	void Update () {
		var jitter_this_time_slice = wanderJitter * Time.deltaTime;
		wanderTarget += new Vector3((Random.value * 2 - 1) * jitter_this_time_slice,
		                            (Random.value * 2 - 1) * jitter_this_time_slice,
		                            (Random.value * 2 - 1) * jitter_this_time_slice);
		wanderTarget.Normalize();
		wanderTarget *= wanderRadius;
		var target = wanderTarget + transform.forward * wanderDistance;
		force = target;

		// Debug rendering
		DebugExtension.DebugWireSphere(
			transform.position + transform.forward * wanderDistance * GetComponent<SphereCollider>().radius,
			Color.green,
			wanderRadius * GetComponent<SphereCollider>().radius);
		DebugExtension.DebugArrow(transform.position, target, Color.red);
		DebugExtension.DebugArrow(Vector3.zero, transform.forward, Color.cyan);
		DebugExtension.DebugArrow(Vector3.zero, wanderTarget + transform.forward * wanderDistance);
	}

	Vector3 PointToWorldSpace(Vector3 point,
	                          Vector3 agent_heading,
	                          Vector3 agent_position) {
		return agent_position + Quaternion.LookRotation(agent_heading) * point;
	}
}