using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Mobile))]
public class Seek : SteeringBehavior {
	public Vector3 target;

	// Update is called once per frame
	void Update () {
		var desired_velocity = (target - transform.position).normalized * GetComponent<Mobile>().maxSpeed;
		force = desired_velocity - GetComponent<Mobile>().velocity;

		DebugExtension.DebugArrow(transform.position, force, Color.blue);
	}
}