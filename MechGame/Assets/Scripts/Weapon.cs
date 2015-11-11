using Vexe.Runtime.Types;
using UnityEngine;

public enum WeaponType {
	Solid,
	Laser,
	Missile,
}

public class Weapon : BetterBehaviour {
	[HideInInspector] public Mech  owner;
	[HideInInspector] public float fireTime = 0;

	public WeaponType type;
	public Transform  spawn;
	public Ammunition ammo;
	public float      cooldown;

	public float MinRange {
		get { return minRange * ammo.rangeMod; }
	}

	public float MaxRange {
		get { return maxRange * ammo.rangeMod; }
	}

	public float Damage {
		get { return ammo.damage; }
	}

	public void Fire() {
		if (fireTime <= 0) {
			fireTime = cooldown;
			var round = (Ammunition)Instantiate(ammo, spawn.position, Quaternion.identity);
			round.Velocity = transform.forward * ammo.fireSpeed;
		}
	}

	float minRange;
	float maxRange;

	void Update() {
		DebugExtension.DebugCircle(owner.transform.position, Color.red, fireTime / cooldown);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
		}
	}
}