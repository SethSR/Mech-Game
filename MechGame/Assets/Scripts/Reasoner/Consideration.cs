using UnityEngine;
using Vexe.Runtime.Types;

public enum ConsiderationTypes {
	Distance,
	Time,
	TeamSize,
	Health,
	Multiplier,
}

public class Consideration {
	[HideInInspector] public Mech               mech;
	                  public ConsiderationTypes type;
	[VisibleWhen(   "isDistance")] public Mech           target;
	[VisibleWhen(       "isTime")] public float          timeLimit;
	[VisibleWhen(   "isDistance")] public float          maxDistance;
	[VisibleWhen( "isMultiplier")] public float          multiplier;
	[VisibleWhen("!isMultiplier")] public AnimationCurve utilCurve;

	bool isDistance  () { return type == ConsiderationTypes.Distance; }
	bool isHealth    () { return type == ConsiderationTypes.Health; }
	bool isTime      () { return type == ConsiderationTypes.Time; }
	bool isMultiplier() { return type == ConsiderationTypes.Multiplier; }
	bool isTeamSize  () { return type == ConsiderationTypes.TeamSize; }

	[Readonly] public virtual float Utility {
		get {
			float result = 0f;

			switch (type) {
				case ConsiderationTypes.Distance: {
					var dir = (target.transform.position - mech.transform.position);
					result = Mathf.Max(Mathf.Min(utilCurve.Evaluate(dir.magnitude / maxDistance), 1), 0);
				} break;

				case ConsiderationTypes.Health: {
					var hp_ratio = mech.CurrentHealth / mech.TotalHealth;
					result = utilCurve.Evaluate(hp_ratio);
				} break;

				case ConsiderationTypes.TeamSize: {
					var cur_team_size = mech.CurrentTeamSize / (float)mech.TotalTeamSize;
					result = utilCurve.Evaluate(cur_team_size);
				} break;

				case ConsiderationTypes.Time: {
					var cur_time = Time.time / startTime;
					var time_ratio = cur_time / timeLimit;
					result = Mathf.Min(utilCurve.Evaluate(time_ratio), 1);
				} break;

				default: {
					// Do nothing
				} break;
			}

			return result;
		}
	}

  public float InvUtility { get { return 1 - Utility; } }

	float startTime;

	void Start() {
		startTime = Time.time;
	}
}