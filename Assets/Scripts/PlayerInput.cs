using InControl;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour {
	public Transform throttle; //TODO(seth): This is temporary for Rule of Cool!
	public Transform control;  //TODO(seth): This is temporary for Rule of Cool!
	// static public float   LeftTrigger     (GamePad.Index playerIndex) { return GamePad.GetTrigger(GamePad.Trigger.LeftTrigger , playerIndex); }
	// static public float   RightTrigger    (GamePad.Index playerIndex) { return GamePad.GetTrigger(GamePad.Trigger.RightTrigger, playerIndex); }
	// static public Vector2 LeftAxis        (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.LeftStick      , playerIndex); }
	// static public Vector2 RightAxis       (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.RightStick     , playerIndex); }
	// static public Vector2 DpadAxis        (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.Dpad           , playerIndex); }
	// static public bool    LeftShoulderBtn (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.LeftShoulder , playerIndex); }
	// static public bool    RightShoulderBtn(GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.RightShoulder, playerIndex); }
	// static public bool    LeftStickBtn    (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.LeftStick    , playerIndex); }
	// static public bool    RightStickBtn   (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.RightStick   , playerIndex); }
	// static public bool    CrossBtn        (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.A            , playerIndex); }
	// static public bool    CircleBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.B            , playerIndex); }
	// static public bool    SquareBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.X            , playerIndex); }
	// static public bool    TriangleBtn     (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Y            , playerIndex); }
	// static public bool    StartBtn        (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Start        , playerIndex); }
	// static public bool    SelectBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Back         , playerIndex); }

	public bool invertedX = false;
	public bool invertedY = false;

	// INPUT -> ACTIONS
	// Left  Shoulder  -> Left Arm/Weapon
	// Right Shoulder  -> Right Arm/Weapon
	// Left  Trigger   -> Left Roll
	// Right Trigger   -> Right Roll
	// Left  Stick Btn -> Boost
	// Right Stick Btn -> ???

	// Left  Stick X -> Strafe Control
	// Left  Stick Y -> Thrust Control
	// Right Stick X -> Yaw
	// Right Stick Y -> Pitch

	// D-Pad -> Mech Modes (combat, salvage, ...)
	// Up    -> Combat Mode
	// Left  -> Salvage Mode
	// Right -> ???
	// Down  -> Low-Power Mode

	// Buttons -> Mode Utilities (repair, scanning, target-switch, ...)
	// A/Cross    -> Repair
	// B/Circle   -> ???
	// X/Square   -> Scan Target/Resource
	// Y/Triangle -> Switch Target/Resource

	// Start  -> Pause Menu
	// Select -> Some-kind-of-galaxy/mech/overview Menu

	void Update() {
		var device = InputManager.ActiveDevice;

		// Throttle        : -0.15 - 0.02
		// Control Stick X : -15 - 15
		// Control Stick Y : -15 - 15
		// Control Stick Z : -15 - 15

		var movement = GetComponent<Movement>();
		if (movement) {
			throttle.position = transform.position + transform.rotation * new Vector3(0.268f,
			                                0.8f-0.1244f,
			                                0.6f-0.0889f + Mathf.Lerp(-0.15f, 0.02f, device.LeftStickY.Value * 0.5f + 0.5f));
			throttle.rotation = transform.rotation;
			control.rotation = transform.rotation * Quaternion.Euler(-Mathf.Lerp(-15, 15, device.RightStickY.Value * 0.5f + 0.5f),
			                                                         Mathf.Lerp(-15, 15, device.RightStickX.Value * 0.5f + 0.5f),
			                                                         Mathf.Lerp(-15, 15, (device.LeftTrigger - device.RightTrigger) * 0.5f + 0.5f));
			movement.SetTorque(invertedY ? device.RightStickY.Value : -device.RightStickY.Value,
			                   invertedX ? -device.RightStickX.Value : device.RightStickX.Value,
			                   device.LeftTrigger - device.RightTrigger);
			movement.SetForce(device.LeftStickX.Value,
			                  device.RightStickButton ? device.LeftStickY.Value : 0,
			                  device.RightStickButton ? 0 : device.LeftStickY.Value);
			movement.SetBoost(device.LeftStickButton);
		}

		//TODO(seth): These will go away eventually!
		var right_weapon = GetComponent<Weapon>();
		if (right_weapon && device.RightBumper) {
			right_weapon.Fire();
		}

		var mech = GetComponent<Mech>();
		if (mech) {
			// if (DpadAxis(playerIndex).y > 0) { mech.SetMode(MechMode.Combat); }
			// if (DpadAxis(playerIndex).x < 0) { mech.SetMode(MechMode.Salvage); }
			// if (DpadAxis(playerIndex).x > 0) { mech.SetMode(MechMode.Unknown); }
			// if (DpadAxis(playerIndex).y < 0) { mech.SetMode(MechMode.LowPower); }

			if (device.LeftBumper) { mech.ActivateLeftArm(); }
			if (device.RightBumper) { mech.ActivateRightArm(); }

			// mech.ActivateAbility1(CrossBtn(playerIndex));
			// mech.ActivateAbility2(CircleBtn(playerIndex));
			// mech.ActivateAbility3(SquareBtn(playerIndex));
			// mech.ActivateAbility4(TriangleBtn(playerIndex));
		}

		// PauseMenu.Activate(StartBtn(playerIndex));
		// OverviewMenu.Activate(SelectBtn(playerIndex));
	}
}