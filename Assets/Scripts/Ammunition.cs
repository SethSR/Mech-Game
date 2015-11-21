using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Ammunition : MonoBehaviour {
	[fMin(0.1f )] public float lifetime;
	[fMin(0.01f)] public float rangeMod;
	public float damage;
	public float fireSpeed;

	public Vector3 Velocity {
		get { return rb.velocity; }
		set { velocity = value; }
	}

	Rigidbody rb;
	Vector3 velocity;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		lifetime -= Time.deltaTime;
		if (lifetime < 0) {
			Destroy(gameObject);
		}
	}

	void FixedUpdate() {
		rb.velocity = velocity;
	}
}