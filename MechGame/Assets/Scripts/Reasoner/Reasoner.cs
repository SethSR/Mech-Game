using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mobile))]
[RequireComponent(typeof(Mech))]
public class Reasoner : BetterBehaviour {
	List<Action>     actions;
	List<Mech>       enemies;
	List<GameObject> others;

	public List<Action> actionDeciders = new List<Action>(5);

	Action currentAction = null;

	public void enactDecision() { //TODO: need someway to use one action on multiple targets
		Action chosen = null;
		var best_utility = 0f;
		foreach (var ad in actionDeciders) {
			var utility = ad.Utility;
			Debug.Log(ad.type + ", utility: " + utility);
			utility     *= ad == currentAction ? ad.commitmentBonus : 1;
			chosen       = best_utility < utility ? ad : chosen;
			best_utility = best_utility < utility ? utility : best_utility;
		}
		chosen.enact();
	}
}