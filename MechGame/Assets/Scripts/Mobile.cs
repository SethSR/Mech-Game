using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Mobile : MonoBehaviour {
	public float maxSpeed;
	public float maxForce;
	public List<ActionDecider> actionDeciders;

	[HideInInspector] public Vector3 velocity = Vector3.forward;

	// Update is called once per frame
	void Update () {
		var steering_force = Vector3.zero;
		foreach (var ad in actionDeciders) {
			var force   = ad.Behavior.Force;
			var utility = ad.considerations.Select(c => c.Utility).Aggregate((a,b) => a + b);
			Debug.Log("Force: " + force + ", Utility: " + utility);
			DebugExtension.DebugArrow(transform.position, force, Color.cyan);
			steering_force += force;
		}
		steering_force = Vector3.ClampMagnitude(steering_force, maxForce);
		DebugExtension.DebugArrow(transform.position, steering_force, Color.black);
		var acceleration = steering_force / GetComponent<Rigidbody>().mass;
		velocity += acceleration * Time.deltaTime;
		velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
		transform.position += velocity * Time.deltaTime;

		transform.rotation = Quaternion.LookRotation(velocity);
		DebugExtension.DebugArrow(transform.position, velocity);
	}
}