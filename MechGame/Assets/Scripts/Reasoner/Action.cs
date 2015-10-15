using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;

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
	          public ActionTypes         actionType;
	[fMin(1)] public float               commitmentBonus;
	          public Mech                mech;
	          public GameObject          target;
	          public List<Consideration> considerations;

	public void enact() {}
}