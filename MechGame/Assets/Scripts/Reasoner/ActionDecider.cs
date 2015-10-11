using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;

public class ActionDecider {
	                  public bool                enable = true;
	                  public string              actionName;
	[fMin(1)]         public float               commitmentBonus;
	                  public Decision            decision;
	                  public List<Consideration> considerations;
	[HideInInspector] public bool                currentAction;

	public void initialize(Mech m, Transform t) {
		decision.mech = m;
		considerations.ForEach(con => con.transform = t);
	}
}