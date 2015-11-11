using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	public float shake;

	void Update() {
		var speed = transform.parent.GetComponent<Rigidbody>().velocity.magnitude;
		if (speed > 0) {
			transform.position = transform.parent.position + transform.parent.rotation * (Vector3.up + Random.insideUnitSphere * Mathf.Lerp(shake, 0, 10/speed));
		}
	}
}