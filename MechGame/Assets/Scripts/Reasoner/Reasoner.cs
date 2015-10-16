using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mech))]
public class Reasoner : BetterBehaviour {
	List<Action>     actions;
	List<Mech>       enemies;
	List<GameObject> others;

	public List<Action> actionDeciders;

	Action currentAction = null;

	void Start() {
		var mech = GetComponent<Mech>();
		actionDeciders.ForEach(ad => ad.mech = mech);
	}

	void Update() {
		var best_utility = -1f;
		foreach (var ad in actionDeciders) {
			var utility = ad.Utility;
			Debug.Log(ad.type + ", utility: " + utility);
			utility      *= ad == currentAction ? ad.commitmentBonus : 1;
			currentAction = best_utility < utility ? ad : currentAction;
			best_utility  = best_utility < utility ? utility : best_utility;
		}
		// Debug.Log(currentAction.type + " chosen");
		currentAction.enact();
	}
}