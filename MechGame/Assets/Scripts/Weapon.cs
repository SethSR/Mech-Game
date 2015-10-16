using Vexe.Runtime.Types;
using UnityEngine;

public class Weapon : BetterBehaviour {
	[HideInInspector] public float fireTime;

	public float cooldown;
	public float minRange;
	public float maxRange;
	public float damage;

	public void fire() {
		fireTime = Time.time;
	}

	void Start() {
		fireTime = Time.time;
	}
}