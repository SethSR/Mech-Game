using GamepadInput;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour {
	static public float   LeftTrigger     (GamePad.Index playerIndex) { return GamePad.GetTrigger(GamePad.Trigger.LeftTrigger , playerIndex); }
	static public float   RightTrigger    (GamePad.Index playerIndex) { return GamePad.GetTrigger(GamePad.Trigger.RightTrigger, playerIndex); }
	static public Vector2 LeftAxis        (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.LeftStick      , playerIndex); }
	static public Vector2 RightAxis       (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.RightStick     , playerIndex); }
	static public Vector2 DpadAxis        (GamePad.Index playerIndex) { return GamePad.GetAxis   (GamePad.Axis.Dpad           , playerIndex); }
	static public bool    LeftShoulderBtn (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.LeftShoulder , playerIndex); }
	static public bool    RightShoulderBtn(GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.RightShoulder, playerIndex); }
	static public bool    LeftStickBtn    (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.LeftStick    , playerIndex); }
	static public bool    RightStickBtn   (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.RightStick   , playerIndex); }
	static public bool    CrossBtn        (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.A            , playerIndex); }
	static public bool    CircleBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.B            , playerIndex); }
	static public bool    SquareBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.X            , playerIndex); }
	static public bool    TriangleBtn     (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Y            , playerIndex); }
	static public bool    StartBtn        (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Start        , playerIndex); }
	static public bool    SelectBtn       (GamePad.Index playerIndex) { return GamePad.GetButton (GamePad.Button.Back         , playerIndex); }

	public GamePad.Index playerIndex;
	public bool          invertedX = false;
	public bool          invertedY = false;

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
		var movement = GetComponent<Movement>();
		movement.SetTorque(invertedY ? RightAxis(playerIndex).y : -RightAxis(playerIndex).y,
		                   invertedX ? -RightAxis(playerIndex).x : RightAxis(playerIndex).x,
		                   LeftTrigger(playerIndex) - RightTrigger(playerIndex));
		movement.SetForce(LeftAxis(playerIndex).x,
		                  RightStickBtn(playerIndex) ? LeftAxis(playerIndex).y : 0,
		                  RightStickBtn(playerIndex) ? 0 : LeftAxis(playerIndex).y);
		movement.SetBoost(LeftStickBtn(playerIndex));
	}
}