using UnityEngine;
using System.Collections;

public class CTeamSize : Consideration {
	public Mech mech;

	public override float Utility {
		get {
			var cur_team_size = mech.CurrentTeamSize / (float)mech.TotalTeamSize;
			return utilCurve.Evaluate(cur_team_size);
		}
	}
}