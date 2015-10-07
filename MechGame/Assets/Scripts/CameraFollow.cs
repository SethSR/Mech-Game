using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public Vector3   offset;
	
	// Update is called once per frame
	void Update () {
		transform.position = target.position + offset;
		transform.rotation = Quaternion.LookRotation(target.position - transform.position);
	}
}