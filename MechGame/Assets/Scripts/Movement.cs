using GamepadInput;
using Vexe.Runtime.Types;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : BetterBehaviour {
	public float angThrust       =  10; // deg/s^2

	public float mainThrust      = 100; // Kg * m/s^2
	public float horzThrust      = 100; // Kg * m/s^2
	public float vertThrust      = 100; // Kg * m/s^2
	public float backThrust      = 100; // Kg * m/s^2
	public float maxSpeed        =  20; // Kg * m/s
	public float mainBoostThrust = 500;
	public float horzBoostThrust = 500;
	public float vertBoostThrust = 500;
	public float backBoostThrust = 500;
	public float maxBoostSpeed   =  40;

	public Vector3 heading {
		get { return transform.forward; }
	}

	public Vector3 velocity {
		get { return rb.velocity; }
	}

	public Vector3 position {
		get { return rb.position; }
	}

	public float speed {
		get { return rb.velocity.magnitude; }
	}

	//NOTE(seth): All SetValue(float) methods expect a value in [0..1]
	public void SetPitch(float p) { pitch = p; }
	public void SetYaw  (float y) { yaw   = y; }
	public void SetRoll (float r) { roll  = r; }

	public void SetXForce(float x) { horz = x; }
	public void SetYForce(float y) { vert = y; }
	public void SetZForce(float z) { main = z; }

	public void SetTorque(float p, float y, float r) {
		SetPitch(p);
		SetYaw  (y);
		SetRoll (r);
	}

	public void SetForce(float x, float y, float z) {
		SetXForce(x);
		SetYForce(y);
		SetZForce(z);
	}

	//NOTE(seth): All SetValue(Vector3) methods expect a normalized vector
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

	public void SetMaxSpeed(float max) {
		maxSpeed = max;
	}

	public void SetMaxAngSpeed(float max) {
		rb.maxAngularVelocity = max;
	}

	public void SetBoost(bool enable) {
		boostOn = enable;
	}

	float pitch = 0f;
	float yaw   = 0f;
	float roll  = 0f;

	float horz = 0f;
	float vert = 0f;
	float main = 0f;

	bool boostOn = false;

	Color headingColor = new Color(0.25f,0.25f,1,1);

	Rigidbody rb;

	void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate() {
		var pitch_torque = pitch * angThrust;
		var yaw_torque   = yaw   * angThrust;
		var roll_torque  = roll  * angThrust;
		rb.AddRelativeTorque(pitch_torque, yaw_torque, roll_torque);

		var horz_force = horz * (horzThrust + (boostOn ? horzBoostThrust : 0));
		var vert_force = vert * (vertThrust + (boostOn ? vertBoostThrust : 0));
		var main_force = main * ((main > 0)
			? (mainThrust + (boostOn ? mainBoostThrust : 0))
			: (backThrust + (boostOn ? backBoostThrust : 0)));
		rb.AddRelativeForce(horz_force, vert_force, main_force);
		rb.velocity = Vector3.ClampMagnitude(velocity, (boostOn ? maxBoostSpeed : maxSpeed));

		DebugExtension.DebugArrow(position, velocity, headingColor);
	}
}