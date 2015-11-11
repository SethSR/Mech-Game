using Vexe.Runtime.Types;
using UnityEngine;

public enum WeaponType {
	Solid,
	Laser,
	Missile,
}

public class Weapon : BetterBehaviour {
	[HideInInspector] public float fireTime = 0;

	public WeaponType type;
	public Transform  spawn;
	public Ammunition ammo;
	public float      cooldown;

	public float MinRange {
		get { return minRange * ammo.rangeMod; }
		set { minRange = value; }
	}

	public float MaxRange {
		get { return maxRange * ammo.rangeMod; }
		set { maxRange = value; }
	}

	public float Damage {
		get { return ammo.damage; }
	}

	public void Fire() {
		if (fireTime <= 0) {
			fireTime = cooldown;
			var round = (Ammunition)Instantiate(ammo, spawn.position, ammo.transform.rotation * transform.rotation);
			round.Velocity = transform.forward * ammo.fireSpeed;
		}
	}

	float minRange;
	float maxRange;

	void Update() {
		DebugExtension.DebugCircle(spawn.transform.position, Color.red, fireTime / cooldown);
		if (fireTime > 0) {
			fireTime -= Time.deltaTime;
		}
	}
}