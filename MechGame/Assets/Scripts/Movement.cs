using GamepadInput;
using Vexe.Runtime.Types;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : BetterBehaviour {
	//NOTE(seth): Remember to set the linear and angular drag in the rigidbody!!!
	// Around 5 seems pretty good for the default values below.
	public float mainThrust  = 100; // Kg * m/s^2
	public float horzThrust  = 100; // Kg * m/s^2
	public float vertThrust  = 100; // Kg * m/s^2
	public float backThrust  = 100; // Kg * m/s^2
	public float maxSpeed    =  20; // Kg * m/s

	public float angThrust   = 10; // deg/s^2

	public Vector3 heading {
		get { return transform.forward; }
	}

	public Vector3 velocity {
		get { return rb.velocity; }
	}

	public Vector3 position {
		get { return transform.position; }
	}

	public float speed {
		get { return Velocity.magnitude; }
	}

	public void SetPitch(float p) { pitch = p * angThrust * -transform.right; }
	public void SetYaw  (float y) { yaw   = y * angThrust *  transform.up; }
	public void SetRoll (float r) { roll  = r * angThrust * -transform.forward; }

	public void SetXForce(float x) { horz = x * horzThrust * transform.right; }
	public void SetYForce(float y) { vert = y * vertThrust * transform.up; }
	public void SetZForce(float z) { main = z * (z > 0 ? mainThrust : backThrust) * transform.forward; }

	public void SetTorque(Vector3 angles) {
		SetPitch(angles.x);
		SetYaw  (angles.y);
		SetRoll (angles.z);
	}

	public void SetForce(Vector3 forces) {
		SetXForce(forces.x);
		SetYForce(forces.y);
		SetZForce(forces.z);
	}

	Vector3 pitch = Vector3.zero;
	Vector3 yaw   = Vector3.zero;
	Vector3 roll  = Vector3.zero;

	Vector3 horz = Vector3.zero;
	Vector3 vert = Vector3.zero;
	Vector3 main = Vector3.zero;

	Color headingColor = new Color(0,0,0.75f,0.75f);

	Rigidbody rb = null;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		rb.AddTorque(pitch + yaw + roll);
		//NOTE(seth): maxAngularSpeed is defaulted in Rigidbody to "7"
		rb.AddForce(horz + vert + main);
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLinSpeed);
		DebugExtension.DebugArrow(position, velocity, headingColor);
	}
}