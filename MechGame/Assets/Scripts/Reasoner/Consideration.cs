using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Consideration {
	public Transform      transform;
	public AnimationCurve utilCurve;

	public abstract float Utility { get; }

	public float InvUtility { get { return 1 - Utility; } }
}