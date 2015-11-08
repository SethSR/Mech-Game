using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mobile))]
public class Mech : BetterBehaviour {
	[HideInInspector] public Transform target;
	[HideInInspector] public float     currentHealth;
	[HideInInspector] public int       currentTeamSize;

	          public bool  playerControlled = false;
	          public int   team             =  -1;
	[fMin(1)] public float totalHealth      = 100;
	[fMin(1)] public float sensorRange      = 100; // in meters
	          public int   maxTeamSize      =   5; //TODO(seth): move this to the mechs carrier/commander

	public Weapon mainWep;
	public Weapon sideWep;
	public Weapon backWep;

	public void fireWeaponAt(Mech enemy) {
		if (CurrentWeapon.fireTime < 0) {
			DebugExtension.DebugArrow(transform.position, enemy.transform.position - transform.position, Color.red);
			enemy.currentHealth -= CurrentWeapon.fire();
		}
	}

	[HideInInspector] public Weapon CurrentWeapon {
		get {
			//TODO(seth): Choose the most useful weapon somehow
			// for now just pick the main weapon
			return mainWep;
		}
	}


	void Start() {
		currentHealth = totalHealth;
		mainWep = Instantiate<Weapon>(mainWep); mainWep.owner = this;
		sideWep = Instantiate<Weapon>(sideWep); sideWep.owner = this;
		backWep = Instantiate<Weapon>(backWep); backWep.owner = this;
	}

	Color sensorRangeColor = new Color(0, 1, 0, 0.75f);

	void Update() {
		DebugExtension.DebugWireSphere(transform.position, sensorRangeColor, sensorRange);
		if (playerControlled) {
			// do stuff
		} else {
			GetComponent<Reasoner>().DecideOnAction();
		}

		if (currentHealth < 0) {
			// has died
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		if (mainWep != null) { Destroy(mainWep.gameObject); }
		if (sideWep != null) { Destroy(sideWep.gameObject); }
		if (backWep != null) { Destroy(backWep.gameObject); }
	}
}