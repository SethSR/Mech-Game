using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mobile))]
public class Reasoner : BetterBehaviour {
	public List<ActionDecider> actionDeciders;

	Mobile mobile;

	void Awake() {
		mobile = GetComponent<Mobile>();
	}

	void Start() {
		foreach (var ad in actionDeciders) {
			ad.behavior.vehicle = mobile;
			foreach (var con in ad.considerations) {
				con.transform = transform;
			}
		}
	}

	public Vector3 Action {
		get {
			var action = Vector3.zero;
			var best_utility = 0f;
			foreach (var ad in actionDeciders) {
				var utility = ad.considerations.Select(c => c.Utility).Aggregate((a,b) => a * b);
				action       = best_utility < utility ? ad.behavior.Force : action;
				best_utility = best_utility < utility ? utility : best_utility;
			}
			return action;
		}
	}
}