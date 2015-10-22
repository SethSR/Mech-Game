using UnityEngine;
using Vexe.Runtime.Types;

public enum ConsiderationTypes {
	Distance,
	EnemyHealth,
	EnemyTeamSize,
	Health,
	LineOfSight,
	Multiplier,
	TeamSize,
	Time,
	WeaponCooldown,
}

public class Consideration {
	public ConsiderationTypes type;

	[VisibleWhen("isTime")]
	public float timeLimit;
	[VisibleWhen("isMultiplier")]
	public float multiplier;
	[VisibleWhen("!isMultiplier","!isLineOfSight", "!isWeaponCooldown")]
	public AnimationCurve utilCurve;
	[VisibleWhen('|',"isLineOfSight","isWeaponCooldown")]
	public bool inverse = false;

	bool isLineOfSight   () { return type == ConsiderationTypes.LineOfSight; }
	bool isMultiplier    () { return type == ConsiderationTypes.Multiplier; }
	bool isTime          () { return type == ConsiderationTypes.Time; }
	bool isWeaponCooldown() { return type == ConsiderationTypes.WeaponCooldown; }

	public float utility(Mech m, Transform t) {
		Mech      mech   = m;
		Transform target = t;
		float     result = 0f;

		switch (type) {
			case ConsiderationTypes.Distance: {
				var dir = (target.position - mech.transform.position);
				result = Mathf.Max(Mathf.Min(utilCurve.Evaluate(dir.magnitude / mech.sensorRange), 1), 0);
			} break;

			case ConsiderationTypes.EnemyHealth: {
				var enemy = target.GetComponent<Mech>();
				var hp_ratio = enemy.currentHealth / enemy.totalHealth;
				result = utilCurve.Evaluate(hp_ratio);
			} break;

			case ConsiderationTypes.EnemyTeamSize: {
				var enemy = target.GetComponent<Mech>();
				var cur_team_size = enemy.currentTeamSize / (float)enemy.maxTeamSize;
				result = utilCurve.Evaluate(cur_team_size);
			} break;

			case ConsiderationTypes.Health: {
				var hp_ratio = mech.currentHealth / mech.totalHealth;
				result = utilCurve.Evaluate(hp_ratio);
			} break;

			case ConsiderationTypes.LineOfSight: {
				var los = Physics.Linecast(mech.transform.position, target.position);
				//  inverse &&  los -> 0
				//  inverse && !los -> 1
				// !inverse &&  los -> 1
				// !inverse && !los -> 0
				result = inverse
					? (los ? 0 : 1)
					: (los ? 1 : 0);
			} break;

			case ConsiderationTypes.TeamSize: {
				var cur_team_size = mech.currentTeamSize / (float)mech.maxTeamSize;
				result = utilCurve.Evaluate(cur_team_size);
			} break;

			case ConsiderationTypes.Time: {
				var cur_time = Time.time - startTime;
				var time_ratio = cur_time / timeLimit;
				result = Mathf.Min(utilCurve.Evaluate(time_ratio), 1);
			} break;

			case ConsiderationTypes.WeaponCooldown: {
				var wep = mech.CurrentWeapon;
				var cooldown_over = wep.fireTime < 0;
				//  inverse &&  cooldown_over -> 0
				//  inverse && !cooldown_over -> 1
				// !inverse &&  cooldown_over -> 1
				// !inverse && !cooldown_over -> 0
				result = inverse
					? (cooldown_over ? 0 : 1)
					: (cooldown_over ? 1 : 0);
			} break;

			default: {
				// Do nothing
			} break;
		}

		return result;
	}

  public float invUtility(Mech m, Transform t) { return 1 - utility(m,t); }

	float startTime;

	void Start() {
		startTime = Time.time;
	}
}