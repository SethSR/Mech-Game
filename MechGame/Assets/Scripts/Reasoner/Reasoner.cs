using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mech))]
public class Reasoner : BetterBehaviour {
	public List<ActionTypes> actionTypes;
	public List<ActionTypes> movementTypes;

	public void DecideOnMovement() { Decide(moves, ref currentMovement); }
	public void DecideOnAction  () { Decide(actions, ref currentAction); }

	List<Action> actions;
	List<Action> moves;
	Action       currentAction;
	Action       currentMovement;
	Mech         mech;

	void Start() {
		mech = GetComponent<Mech>();
		actions = actionTypes.Select  (at => Action.createType(at)).ToList();
		moves   = movementTypes.Select(mt => Action.createType(mt)).ToList();
	}

	void Decide(List<Action> decisions, ref Action current) {
		var best_utility = -1f;
		foreach (var dd in decisions) {
			var utility = dd.utility(mech);
			utility     *= dd == current ? dd.commitmentBonus : 1;
			current      = best_utility < utility ? dd : current;
			best_utility = best_utility < utility ? utility : best_utility;
		}
		if (current != null) {
			current.enact(mech);
		} else {
			Debug.Log(name + " | action not set!");
		}
		// Debug.Log(name + " | Action: " + current.type + ", Utility: " + best_utility);
	}
}