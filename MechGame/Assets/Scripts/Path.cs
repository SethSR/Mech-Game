using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;

public class Path : BetterBehaviour {
	public bool  isLooped;
	public int   numberOfWaypoints;

	public Vector3 CurrentWaypoint {
		get { return waypoints[current]; }
	}

	public bool Finished {
		get { return isLooped ? false : (current == numberOfWaypoints); }
	}

	public void SetNextWaypoint() {
		++current;
		if (current > numberOfWaypoints) {
			current = isLooped ? 0 : numberOfWaypoints;
		}
	}

	public List<Vector3> waypoints = new List<Vector3>();
	int           current   =  0;
	float waypointDistance  = 10;

	void Start() {
		waypoints.Add(Quaternion.Euler(Random.Range(-45,45), Random.Range(-45,45), 0) * Vector3.forward * waypointDistance);
		for (int i = 1; i <= numberOfWaypoints; ++i) {
			waypoints.Add(waypoints[i-1] + Quaternion.LookRotation(waypoints[i-1]) * Quaternion.Euler(Random.Range(-45,45), Random.Range(-45,45), 0) * Vector3.forward * waypointDistance);
		}
	}

	void Update() {
		for (int i = 1; i <= numberOfWaypoints; ++i) {
			DebugExtension.DebugArrow(waypoints[i-1], waypoints[i] - waypoints[i-1], Color.black);
		}
	}
}