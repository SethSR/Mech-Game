using Vexe.Runtime.Types;
using UnityEngine;

public class Weapon : BetterBehaviour {
	[HideInInspector] public Mech  owner;
	[HideInInspector] public float fireTime;

	public float cooldown;
	public float minRange;
	public float maxRange;
	public float damage;

	public float fire() {
		fireTime = cooldown;
		return damage;
	}

	void Start() {
		fireTime = cooldown;
	}

	void Update() {
		DebugExtension.DebugCircle(owner.transform.position, fireTime / cooldown);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
		}
	}
}