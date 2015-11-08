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
	public float maxLinSpeed =  20; // Kg * m/s

	public float angThrust   = 10; // deg/s^2

	Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	public void setPitch(float p) { pitch = p * angThrust * -transform.right; }
	public void setYaw  (float y) { yaw   = y * angThrust *  transform.up; }
	public void setRoll (float r) { roll  = r * angThrust * -transform.forward; }

	public void setXForce(float x) { horz = x * horzThrust * transform.right; }
	public void setYForce(float y) { vert = y * vertThrust * transform.up; }
	public void setZForce(float z) { main = z * (z > 0 ? mainThrust : backThrust) * transform.forward; }

	public void setTorque(Vector3 angles) {
		setPitch(angles.x);
		setYaw  (angles.y);
		setRoll (angles.z);
	}

	public void setForce(Vector3 forces) {
		setXForce(forces.x);
		setYForce(forces.y);
		setZForce(forces.z);
	}

	Vector3 pitch = Vector3.zero;
	Vector3 yaw   = Vector3.zero;
	Vector3 roll  = Vector3.zero;

	Vector3 horz = Vector3.zero;
	Vector3 vert = Vector3.zero;
	Vector3 main = Vector3.zero;

	void FixedUpdate() {
		rb.AddTorque(pitch + yaw + roll);
		rb.AddForce(horz + vert + main);
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxLinSpeed);
	}
}