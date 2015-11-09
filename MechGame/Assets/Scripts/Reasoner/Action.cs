using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using SteeringBehavior;

public enum ActionTypes {
	Attack,
	Collect,
	Charge,
	Defend,
	Destroy,
	Dodge,
	EMP,
	Evade,
	Flee,
	Hide,
	Idle,
	Move,
	Shield
}

[System.Serializable]
public class Action : BetterScriptableObject {
	static public Action createType(ActionTypes at) {
		if (!actionMap.ContainsKey(at)) {
			actionMap[at] = Resources.Load<Action>("Actions/" + at.ToString());
		}
		return actionMap[at];
	}

	public ActionTypes type;

	[fMin(1)]
	[VisibleWhen("!isIdle")] public float               commitmentBonus;
	[VisibleWhen("!isIdle")] public List<Consideration> considerations;
	[VisibleWhen( "isIdle")] public float               maxIdleValue    = 0.5f;

	public void enact(Mech mech) {
		switch (type) {
			case ActionTypes.Idle: {
			} break;

			case ActionTypes.Attack: {
			} break;

			case ActionTypes.Hide: {
			} break;

			case ActionTypes.Evade: {
			} break;

			case ActionTypes.Flee: {
			} break;

			case ActionTypes.Shield: {
				// do actions
			} break;

			case ActionTypes.EMP: {
				// do actions
			} break;

			case ActionTypes.Charge: {
				// do actions
			} break;

			case ActionTypes.Dodge: {
			} break;

			case ActionTypes.Collect: {
				// do actions
			} break;

			case ActionTypes.Defend: {
				// do actions
			} break;

			case ActionTypes.Destroy: {
				// do actions
			} break;

			case ActionTypes.Move: {
				// do actions
			} break;

			default: {
				// do nothing
			} break;
		}
	}

	public float utility(Mech mech) {
		switch (type) {
			case ActionTypes.Idle: {
				return maxIdleValue;
			}

			case ActionTypes.Attack: {
				float best_utility = 0;
				// foreach (Mech enemy in ec.mechs) {
				// 	if ((enemy.transform.position - mech.transform.position).sqrMagnitude < mech.sensorRange * mech .sensorRange) {
				// 		var utility = compensatedScore(considerations.Select(cons => {
				// 			var con_utility = cons.utility(mech, enemy.transform);
				// 			// Debug.Log(enemy.name + " | " + cons.type + ": " + con_utility);
				// 			return con_utility;
				// 		}));
				// 		mech.target  = (utility > best_utility) ? enemy.transform : mech.target;
				// 		best_utility = (utility > best_utility) ? utility : best_utility;
				// 	}
				// }
				// Debug.Log(mech.name + " | " + type + ": " + best_utility);
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

			case ActionTypes.Collect: {
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

			case ActionTypes.Move: {
				var best_utility = 0;
				return best_utility;
			}

			default: {
				var best_utility = 0;
				return best_utility;
			}
		}
	}

	static EntityContainer ec = null;

	static Dictionary<ActionTypes,Action> actionMap = new Dictionary<ActionTypes,Action>();

	static float compensatedScore(IEnumerable<float> scores) {
		var score       = scores.Aggregate((a,b) => a * b);
		var modFactor   = 1 - (1 / scores.Count());
		var makeUpValue = (1 - score) * modFactor;
		return score + (makeUpValue * score);
	}

	bool isIdle() { return type == ActionTypes.Idle; }

	void Awake() {
		if (ec == null) {
			var go = GameObject.FindWithTag("EntityContainer");
			if (go != null) {
				ec = go.GetComponent<EntityContainer>();
			}
		}
	}
}