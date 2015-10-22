using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mech))]
public class Reasoner : BetterBehaviour {
	public List<ActionTypes> actionTypes;

	List<Action> actions;
	Action currentAction;

	void Start() {
		var mech = GetComponent<Mech>();
		actions = actionTypes.Select(at => {
			Action a = Action.createType(at);
			a.mech = mech;
			return a;
		}).ToList();
	}

	void Update() {
		var best_utility = -1f;
		foreach (var ad in actions) {
			var utility = ad.Utility;
			utility      *= ad == currentAction ? ad.commitmentBonus : 1;
			currentAction = best_utility < utility ? ad : currentAction;
			best_utility  = best_utility < utility ? utility : best_utility;
		}
		if (currentAction != null) {
			currentAction.enact();
		} else {
			Debug.Log(name + " | currentAction not set!");
		}
		// Debug.Log(name + " | Action: " + currentAction.type + ", Utility: " + best_utility);
	}

	void OnDestroy() {
		// actions.ForEach(action => {
		// 	if (action != null) { Destroy(action.gameObject); }
		// });
	}
}