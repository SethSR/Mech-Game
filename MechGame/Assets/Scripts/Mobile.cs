using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Mobile : BetterBehaviour {
	[HideInInspector] public bool    ignoreLimits = false;

	[iMin(1)] public int numberOfSmoothingValues = 20;
	public float boostSpeed = 10;
	public float maxSpeed = 5;
	public float maxForce = 2;

	public Vector3 Heading {
		get { return transform.forward; }
	}

	public float Speed {
		get { return Velocity.magnitude; }
	}

	public Vector3 Velocity {
		get { return rb.velocity; }
	}

	public Vector3 Position {
		get { return transform.position; }
	}

	// Update is called once per frame
	public void update(Vector3 steering_force) {
		if (!ignoreLimits) {
			steering_force = Vector3.ClampMagnitude(steering_force, maxForce);
		}
		var acceleration = steering_force / rb.mass;
		rb.velocity += acceleration * Time.deltaTime;
		if (!ignoreLimits) {
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}
	}

	Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		DebugExtension.DebugArrow(transform.position, rb.velocity, Color.blue);
	}
}