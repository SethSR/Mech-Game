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
			var utility = compensatedScore(ad.considerations.Select(c => c.Utility));
			Debug.Log(ad.actionType + ", utility: " + utility);
			utility     *= ad == currentAction ? ad.commitmentBonus : 1;
			chosen       = best_utility < utility ? ad : chosen;
			best_utility = best_utility < utility ? utility : best_utility;
		}
		chosen.enact();
	}

	static float compensatedScore(IEnumerable<float> scores) {
		var score       = scores.Aggregate((a,b) => a * b);
		var modFactor   = 1 - (1 / scores.Count());
		var makeUpValue = (1 - score) * modFactor;
		return score + (makeUpValue * score);
	}
}

// public class Reasoner {
// 	List<Action>     actions;
// 	List<Mech>       enemies;
// 	List<GameObject> others;
// 	public Action decide() {
// 		Action chosen = null;

// 		foreach (Action action in actions) {
// 			switch (action.actionType) {
// 				case Idle: {
// 					//
// 				} break;

// 				case Attack: {
// 					//
// 				} break;

// 				case Hide: {
// 					//
// 				} break;

// 				case Flee: {
// 					//
// 				} break;

// 				case Shield: {
// 					//
// 				} break;

// 				case EMP: {
// 					//
// 				} break;

// 				case Charge: {
// 					//
// 				} break;

// 				case Dodge: {
// 					//
// 				} break;

// 				case Capture: {
// 					//
// 				} break;

// 				case Defend: {
// 					//
// 				} break;

// 				case Destroy: {
// 					//
// 				} break;

// 				default: {
// 					//
// 				} break;
// 			}
// 		}
// 	}
// }

// Action attack  = { actionType = attack  };
// Action hide    = { actionType = hide    };
// Action collect = { actionType = collect };
// Action retreat = { actionType = retreat };

// Reasoner rsn;
// rsn.add( attack);
// rsn.add(   hide);
// rsn.add(collect);
// rsn.add(retreat);

// Action chosen = rsn.decide()