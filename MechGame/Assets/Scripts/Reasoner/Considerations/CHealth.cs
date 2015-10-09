using UnityEngine;
using System.Collections;

public class CHealth : Consideration {
	public Mech mech;

	public override float Utility {
		get {
			var hp_ratio = mech.CurrentHealth / mech.TotalHealth;
			return utilCurve.Evaluate(hp_ratio);
		}
	}
}