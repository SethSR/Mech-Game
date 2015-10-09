using UnityEngine;

public class CTime : Consideration {
	public float timeLimit;

	void Start() {
		startTime = Time.time;
	}
	
	float startTime;

	public override float Utility {
		get {
			var cur_time = Time.time - startTime;
			var time_ratio = cur_time / timeLimit;
			return Mathf.Min(utilCurve.Evaluate(time_ratio), 1);
		}
	}
}