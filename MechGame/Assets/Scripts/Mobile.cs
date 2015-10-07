using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour {
	public float maxSpeed;
	public float maxForce;

	[HideInInspector] public Vector3 velocity = Vector3.forward;
	
	// Update is called once per frame
	void Update () {
		var steering_force = Vector3.zero;
		foreach (SteeringBehavior sb in GetComponents<SteeringBehavior>()) {
			steering_force += sb.force;
		}
		steering_force = Vector3.ClampMagnitude(steering_force, maxForce);
		var acceleration = steering_force; // would divide by mass
		velocity += acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		transform.position += velocity * Time.deltaTime;

		transform.rotation = Quaternion.LookRotation(velocity);
		DebugExtension.DebugArrow(transform.position, velocity);
	}
}