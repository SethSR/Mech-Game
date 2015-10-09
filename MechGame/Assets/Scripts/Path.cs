using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {
	public Vector3 CurrentWaypoint { get; set; }
	public bool Finished { get; set; }

	public Vector3 SetNextWaypoint() { return Vector3.zero; }
}