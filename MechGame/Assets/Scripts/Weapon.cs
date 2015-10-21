using Vexe.Runtime.Types;
using UnityEngine;

public class Weapon : BetterBehaviour {
	[HideInInspector] public float fireTime;

	public float cooldown;
	public float minRange;
	public float maxRange;
	public float damage;

	public float fire() {
		fireTime = Time.time;
		return damage;
	}

	void Start() {
		fireTime = Time.time;
	}
}