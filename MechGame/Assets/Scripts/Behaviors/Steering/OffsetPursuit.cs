using UnityEngine;
using System.Collections;

public class OffsetPursuit : SteeringBehavior {
	public Mobile  leader;
	public Vector3 offset;

	public override Vector3 Force {
		get {
			var world_offset_pos = leader.transform.position + Quaternion.LookRotation(leader.transform.forward) * offset;
			var to_offset        = world_offset_pos - vehicle.transform.position;
			var look_ahead_time  = to_offset.magnitude / (vehicle.maxSpeed + leader.velocity.magnitude);

			return new Arrive(world_offset_pos + leader.velocity * look_ahead_time,
			                  Deceleration.fast).Force;
		}
	}
}