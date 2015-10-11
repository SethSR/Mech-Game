using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Mobile))]
[RequireComponent(typeof(Mech))]
public class Reasoner : BetterBehaviour {
	public List<ActionDecider> actionDeciders;

	void Start() {
		actionDeciders.ForEach(ad => ad.initialize(GetComponent<Mech>(), transform));
	}

	public void enactDecision() {
		Decision decision = null;
		var best_utility = 0f;
		foreach (var ad in actionDeciders) {
			if (ad.enable) {
				var utility  = compensatedScore(
					ad.considerations.Select(c => c.Utility).Aggregate((a,b) => a * b),
					ad.considerations.Count);
				Debug.Log(ad.actionName + ", utility: " + utility);
				utility         *= ad.currentAction ? ad.commitmentBonus : 1;
				decision         = best_utility < utility ? ad.decision : decision;
				best_utility     = best_utility < utility ? utility : best_utility;
				ad.currentAction = false;
			}
		}
		actionDeciders.Find(ad => ad.decision == decision).currentAction = true;
		decision.enact();
	}

	static float compensatedScore(float score, float numConsiderations) {
		var modFactor   = 1 - (1 / numConsiderations);
		var makeUpValue = (1 - score) * modFactor;
		return score + (makeUpValue * score);
	}
}