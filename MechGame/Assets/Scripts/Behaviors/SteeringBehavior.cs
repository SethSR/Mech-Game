using UnityEngine;
using System;

[Serializable]
public class SteeringBehavior {
	[HideInInspector] public Mobile vehicle;

	public virtual Vector3 Force { get { return Vector3.zero; } }
}