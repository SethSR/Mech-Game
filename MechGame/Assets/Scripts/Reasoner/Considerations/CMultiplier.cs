using UnityEngine;

public class CMultiplier : Consideration {
	public float value;

	public override float Utility {
		get { return value; }
	}
}