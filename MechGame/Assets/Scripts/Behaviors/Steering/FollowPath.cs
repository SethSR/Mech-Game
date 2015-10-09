using UnityEngine;
using System.Collections;

public class FollowPath : SteeringBehavior {
	public Path  path;
	public float sqrWaypointSeekDist;

	public override Vector3 Force {
		get {
			if ((path.CurrentWaypoint - vehicle.transform.position).sqrMagnitude < sqrWaypointSeekDist) {
				path.SetNextWaypoint();
			}

			if (!path.Finished) {
				return new Seek(path.CurrentWaypoint).Force;
			} else {
				return new Arrive(path.CurrentWaypoint, Deceleration.normal).Force;
			}
		}
	}
}