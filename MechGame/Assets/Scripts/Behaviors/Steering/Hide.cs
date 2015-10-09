using UnityEngine;
using System.Collections.Generic;

public class Hide : SteeringBehavior {
	public Mobile          hunter;
	public List<Transform> obstacles;

	public override Vector3 Force {
		get {
			var dist_to_closest = float.MaxValue;
			Vector3 best_hiding_spot;

			foreach (Transform cur_ob in obstacles) {
				var hiding_spot = GetHidingPosition(cur_ob.position,
				                                    cur_ob.GetComponent<SphereCollider>().radius,
				                                    hunter.transform.position);

				var dist = (hiding_spot - vehicle.transform.position).sqrMagnitude;
				if (dist < dist_to_closest) {
					dist_to_closest  = dist;
					best_hiding_spot = hiding_spot;
				}
			}

			if (dist_to_closest == float.MaxValue) {
				return new Evade(hunter).Force;
			} else {
				return new Arrive(best_hiding_spot, Deceleration.fast).Force;
			}
		}
	}

	static Vector3 GetHidingPosition(Vector3 pos_ob,
	                                 float   radius_ob,
	                                 Vector3 pos_hunter) {
		const float distance_from_boundary = 30f;
		var dist_away = radius_ob + distance_from_boundary;

		var to_ob = (pos_ob - pos_hunter).normalized;

		return (to_ob * dist_away) + pos_ob;
	}
}