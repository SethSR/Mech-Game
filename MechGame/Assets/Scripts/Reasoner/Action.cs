using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ActionTypes {
	Idle,
	Attack,
	Hide,
	Flee,
	Shield,
	EMP,
	Charge,
	Dodge,
	Capture,
	Defend,
	Destroy,
}

public class Action : BetterBehaviour {
	[HideInInspector] public Transform           target;
	                  public ActionTypes         type;
	[fMin(1)]         public float               commitmentBonus;
	                  public Mech                mech;
	                  public List<Consideration> considerations;

	void Start() {
		considerations.ForEach(cons => cons.mech = mech);
	}

	public void enact() {}

	public float Utility {
		get {
			switch (type) {
				case ActionTypes.Idle: {
					return compensatedScore(considerations.Select(cons => cons.Utility));
				}

				case ActionTypes.Attack: {
					float best_utility = 0;
					foreach (Mech enemy in mech.enemyMechs) {
						var utility = compensatedScore(considerations.Select(cons => {
							cons.target = enemy;
							return cons.Utility;
						}));
						target       = (utility > best_utility) ? enemy.transform : target;
						best_utility = (utility > best_utility) ? utility : best_utility;
					}
					return best_utility;
				}

				case ActionTypes.Hide: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Flee: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Shield: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.EMP: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Charge: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Dodge: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Capture: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Defend: {
					var best_utility = 0;
					return best_utility;
				}

				case ActionTypes.Destroy: {
					var best_utility = 0;
					return best_utility;
				}

				default: {
					var best_utility = 0;
					return best_utility;
				}
			}
		}
	}

	static float compensatedScore(IEnumerable<float> scores) {
		var score       = scores.Aggregate((a,b) => a * b);
		var modFactor   = 1 - (1 / scores.Count());
		var makeUpValue = (1 - score) * modFactor;
		return score + (makeUpValue * score);
	}
}