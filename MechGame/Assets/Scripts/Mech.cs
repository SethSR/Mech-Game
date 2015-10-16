using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Mech : BetterBehaviour {
	[HideInInspector] public List<Mech> enemyMechs      = new List<Mech>(10);
	[HideInInspector] public float      currentHealth   = 100;
	[HideInInspector] public int        currentTeamSize =   0;
	
	          public bool  playerControlled = false;
	          public int   team             =  -1;
	          public float totalHealth      = 100;
	[fMin(1)] public float sensorRange      = 100; // in meters
	          public int   maxTeamSize      =   5;

	public Weapon mainWep;
	public Weapon sideWep;
	public Weapon backWep;

	public void fireWeaponAt(Mech enemy) {
		DebugExtension.DrawArrow(transform.position, enemy.transform.position, Color.red);
	}

	[HideInInspector] public Weapon CurrentWeapon {
		get {
			//TODO(seth): Choose the most useful weapon somehow
			// for now just pick the main weapon
			return mainWep;
		}
	}


	void Update() {
		if (playerControlled) {
			// do stuff
		} else {
			// do other stuff
		}
	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
			case "Resource":
				// IResource res = other.gameObject.GetComponent<IResource>();
				// interactables.Add(res);
				break;
			case "Mech":
				var mech = other.gameObject.GetComponent<Mech>();
				if (mech.team != team) {
					enemyMechs.Add(mech);
				}
				break;
		}
	}

	void OnTriggerExit(Collider other) {
		switch (other.tag) {
			case "Resource":
				// interactables.Remove(other.gameObject.GetComponent<IResource>());
				break;
			case "Mech":
				var mech = other.gameObject.GetComponent<Mech>();
				if (mech.team != team) {
					enemyMechs.Remove(other.gameObject.GetComponent<Mech>());
				}
				break;
		}
	}
}