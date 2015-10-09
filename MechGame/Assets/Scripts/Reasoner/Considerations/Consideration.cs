using UnityEngine;

public class Consideration {
	[HideInInspector] public Transform      transform;
	                  public AnimationCurve utilCurve;

	public virtual float Utility { get { return 0; } }

  public float InvUtility { get { return 1 - Utility; } }
}