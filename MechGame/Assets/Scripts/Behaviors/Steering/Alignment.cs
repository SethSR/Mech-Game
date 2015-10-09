using UnityEngine;
using System.Collections.Generic;

public class Alignment : SteeringBehavior {
	public List<Mobile> neighbors;

	public override Vector3 Force {
		get {
			var average_heading = Vector3.zero;

			var neighbor_count = 0;

			foreach (var neighbor in neighbors) {
				if (neighbor != vehicle) {
					average_heading += neighbor.transform.forward;
					++neighbor_count;
				}
			}

			if (neighbor_count > 0) {
				average_heading /= neighbor_count;
				average_heading -= vehicle.transform.forward;
			}

			return average_heading;
		}
	}
}