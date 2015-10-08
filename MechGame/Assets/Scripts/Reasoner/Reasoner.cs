using UnityEngine;
using System.Collections.Generic;

public class Reasoner : MonoBehaviour {
	public class ActionDecider {
		public string              actionName;
		public SteeringBehavior    behavior;
		public List<Consideration> considerations;
	}

	public List<ActionDecider> actionDeciders;
}