using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public Vector3   offset;
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + offset;
		var to_target = (target.position - transform.position);
		if (to_target != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(to_target);
		}
	}
}