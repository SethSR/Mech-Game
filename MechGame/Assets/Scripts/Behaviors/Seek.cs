using UnityEngine;
using System;

[RequireComponent(typeof(Mobile))]
public class Seek : SteeringBehavior {
	public Transform target;

	// Update is called once per frame
	public override Vector3 Force {
    get {
  		var desired_velocity = (target.position - vehicle.transform.position).normalized * vehicle.maxSpeed;
      var force = desired_velocity - vehicle.velocity;
      // DebugExtension.DebugArrow(vehicle.transform.position, force, Color.blue);
  		return force;
    }
	}
}