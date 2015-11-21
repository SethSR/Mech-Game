using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Mech : BetterBehaviour {
	[HideInInspector] public Transform target;
	[HideInInspector] public float     currentHealth;
	[HideInInspector] public int       currentTeamSize;

	          public bool  playerControlled = false;
	          public int   team             =  -1;
	[fMin(1)] public float totalHealth      = 100;
	[fMin(1)] public float sensorRange      = 100; // in meters
	          public int   maxTeamSize      =   5; //TODO(seth): move this to the mechs carrier/commander

	public void ActivateLeftArm() {
		Debug.Log(name + " activate left arm equipment");
	}

	public void ActivateRightArm() {
		Debug.Log(name + " activate right arm equipment");
	}


	void Start() {
		currentHealth = totalHealth;
	}

	Color sensorRangeColor = new Color(0, 1, 0, 0.75f);

	void Update() {
		// DebugExtension.DebugWireSphere(transform.position, sensorRangeColor, sensorRange);
		if (currentHealth < 0) {
			// has died
			Destroy(gameObject);
		}
	}
}