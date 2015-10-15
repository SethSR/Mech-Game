using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Mech : BetterBehaviour {
	[HideInInspector] public List<Mech> enemyMechs = new List<Mech>(10);
	                  public int        team       = -1;

	[HideInInspector] public float CurrentHealth   { get; set; }
	                  public float TotalHealth     { get; set; }
	                  public int   CurrentTeamSize { get; set; }
	                  public int   TotalTeamSize   { get; set; }

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