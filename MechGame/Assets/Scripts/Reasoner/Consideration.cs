using UnityEngine;

public enum ConsiderationTypes {
	Distance,
	Time,
	TeamSize,
	Health,
}

public class Consideration {
	[HideInInspector] public Mech               mech;
	                  public ConsiderationTypes type;
	                  public Mech               target;
	                  public float              timeLimit;
	                  public float              maxDistance;
	                  public float              multiplier;
	                  public AnimationCurve     utilCurve;

	public virtual float Utility {
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