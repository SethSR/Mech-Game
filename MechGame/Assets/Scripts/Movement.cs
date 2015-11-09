using GamepadInput;
using Vexe.Runtime.Types;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : BetterBehaviour {
	public float angThrust       =  10; // deg/s^2
	public float angDrag         =   0.5f;

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
	public float linDrag         =   0.5f;

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

	Color headingColor = new Color(0.25f,0.25f,1,0.75f);

	Rigidbody rb = null;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		rb.angularDrag = 0;
		rb.drag = 0;
	}

	void FixedUpdate() {
		var pitch_torque = pitch * angThrust;
		var yaw_torque   = yaw   * angThrust;
		var roll_torque  = roll  * angThrust;

		var torque = new Vector3(pitch_torque, yaw_torque, roll_torque);

		if (torque.sqrMagnitude > 0) {
			rb.AddRelativeTorque(torque);
		} else {
			var frc_val         = Mathf.Min(rb.angularVelocity.magnitude, angDrag);
			var sign            = rb.angularVelocity.normalized;
			var frc_in_cur_dir  = frc_val * sign;
			rb.angularVelocity -= frc_in_cur_dir;
		}

		var horz_force = horz * (horzThrust + (boostOn ? horzBoostThrust : 0));
		var vert_force = vert * (vertThrust + (boostOn ? vertBoostThrust : 0));
		var m_force = (main > 0)
			? (mainThrust + (boostOn ? mainBoostThrust : 0))
			: (backThrust + (boostOn ? backBoostThrust : 0));
		var main_force = main * m_force;

		var force = new Vector3(horz_force, vert_force, main_force);

		//TODO(seth): implement Sonic-esque acceleration
		var pressing_a_direction = force.sqrMagnitude > 0;
		if (pressing_a_direction) {
			if (speed < maxSpeed) {
				rb.AddRelativeForce(force);
			} else {
				// alignment is in the range [0..1]
				var alignment = 0.5f + Vector3.Dot(force.normalized, velocity.normalized) * 0.5f;
				rb.AddRelativeForce(force * (1 - alignment));
			}
		} else {
			// get scaled drag value
			// if speed is less than drag, lower speed to zero
			var frc_val = Mathf.Min(speed, linDrag);
			// get the direction of the velocity
			var sign = velocity.normalized;
			// the friction against the current direction
			var frc_in_cur_dir = frc_val * sign;
			// apply the friction to the current velocity
			rb.velocity -= frc_in_cur_dir;
		}
		DebugExtension.DebugArrow(position, velocity, headingColor);
	}
}