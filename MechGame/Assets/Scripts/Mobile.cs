﻿using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Mobile : BetterBehaviour {
	public float maxSpeed = 5;
	public float maxForce = 2;

	[HideInInspector] public Vector3 velocity = Vector3.forward;

	// Update is called once per frame
	public void update(Vector3 steering_force) {
		steering_force = Vector3.ClampMagnitude(steering_force, maxForce);
		var acceleration = steering_force / GetComponent<Rigidbody>().mass;
		velocity += acceleration * Time.deltaTime;
		velocity  = Vector3.ClampMagnitude(velocity, maxSpeed);
	}

	void Update() {
		transform.position += velocity * Time.deltaTime;
		transform.rotation  = Quaternion.LookRotation(velocity != Vector3.zero ? velocity : transform.forward);
		DebugExtension.DebugArrow(transform.position, velocity);
	}
}