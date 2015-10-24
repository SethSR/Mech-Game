using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class Mobile : BetterBehaviour {
	[HideInInspector] public Vector3 velocity = Vector3.forward;

	[iMin(1)] public int numberOfSmoothingValues = 20;
	public float maxSpeed = 5;
	public float maxForce = 2;

	public bool enableTesting = false;
	[VisibleWhen("enableTesting")] public bool enableAlignment = false;
	[VisibleWhen("enableTesting")] public bool enableCohesion = false;
	[VisibleWhen("enableTesting")] public bool enableSeparation = false;

	[VisibleWhen("enableTesting")] public bool enableArrive = false;
	[VisibleWhen("enableArrive")] public Deceleration deceleration = Deceleration.normal;
	[VisibleWhen("enableArrive")] public Vector3 arriveTarget;

	[VisibleWhen("enableTesting")] public bool enableFlee = false;
	[VisibleWhen("enableFlee")] public Vector3 fleeTarget;

	[VisibleWhen("enableTesting")] public bool enableSeek = false;
	[VisibleWhen("enableSeek")] public Vector3 seekTarget;

	[VisibleWhen("enableTesting")] public bool enableEvade = false;
	[VisibleWhen("enableEvade")] public Mobile pursuer;
	[VisibleWhen("enableEvade")] public float  threatRange;

	[VisibleWhen("enableTesting")] public bool enableFollowPath = false;
	[VisibleWhen("enableFollowPath")] public Path  path;
	[VisibleWhen("enableFollowPath")] public float waypointSeekDist;

	[VisibleWhen("enableTesting")] public bool enableHide = false;
	[VisibleWhen("enableHide")] public Mobile hunter;

	[VisibleWhen("enableTesting")] public bool enableInterpose = false;
	[VisibleWhen("enableInterpose")] public Mobile agentA;
	[VisibleWhen("enableInterpose")] public Mobile agentB;

	[VisibleWhen("enableTesting")] public bool enableObstacleAvoidance = false;
	[VisibleWhen("enableObstacleAvoidance")] public float minDetectionBoxLength;

	[VisibleWhen("enableTesting")] public bool enableOffsetPursuit = false;
	[VisibleWhen("enableOffsetPursuit")] public Mobile  leader;
	[VisibleWhen("enableOffsetPursuit")] public Vector3 offset;

	[VisibleWhen("enableTesting")] public bool enablePursuit = false;
	[VisibleWhen("enablePursuit")] public Mobile evader;

	[VisibleWhen("enableTesting")] public bool enableWallAvoidance = false;
	[VisibleWhen("enableWallAvoidance")] public float wallFeelerLength;

	[VisibleWhen("enableTesting")] public bool enableWander = false;
	[VisibleWhen("enableWander")] public float wanderJitter;
	[VisibleWhen("enableWander")] public float wanderRadius;
	[VisibleWhen("enableWander")] public float wanderDistance;

	HashSet<Mobile>    nearbyVehicles = new HashSet<Mobile>();    // alignment, cohesion, separation
	HashSet<Transform> obstacles      = new HashSet<Transform>(); // obstacle avoidance
	HashSet<Transform> walls          = new HashSet<Transform>(); // wall avoidance
	Vector3            wanderTarget   = Vector3.forward;          // wander
	VecSmoother        smoothRotation;

	void Start() {
		smoothRotation = new VecSmoother(numberOfSmoothingValues);
	}

	void OnTriggerEnter(Collider other) {
		switch (other.tag) {
			case "Mech": {
				Mobile mech = other.gameObject.GetComponent<Mobile>();
				nearbyVehicles.Add(mech);
			} break;

			case "Obstacle": {
				Transform obs = other.gameObject.GetComponent<Transform>();
				obstacles.Add(obs);
			} break;

			case "Wall": {
				Transform wall = other.gameObject.GetComponent<Transform>();
				walls.Add(wall);
			} break;
		}
	}

	void OnTriggerExit(Collider other) {
		switch (other.tag) {
			case "Mech": {
				nearbyVehicles.Remove(other.gameObject.GetComponent<Mobile>());
			} break;

			case "Obstacle": {
				obstacles.Remove(other.gameObject.GetComponent<Transform>());
			} break;

			case "Wall": {
				walls.Remove(other.gameObject.GetComponent<Transform>());
			} break;
		}
	}

	Vector3 CalculateForces {
		get {
			var steering_force = Vector3.zero;
			if (enableAlignment)         { steering_force += SteeringBehavior.Alignment        (this, nearbyVehicles); }
			if (enableArrive)            { steering_force += SteeringBehavior.Arrive           (this, arriveTarget, deceleration); }
			if (enableCohesion)          { steering_force += SteeringBehavior.Cohesion         (this, nearbyVehicles); }
			if (enableEvade)             { steering_force += SteeringBehavior.Evade            (this, pursuer, threatRange); }
			if (enableFlee)              { steering_force += SteeringBehavior.Flee             (this, fleeTarget); }
			if (enableFollowPath)        { steering_force += SteeringBehavior.FollowPath       (this, path, waypointSeekDist); }
			if (enableHide)              { steering_force += SteeringBehavior.Hide             (this, hunter, obstacles); }
			if (enableInterpose)         { steering_force += SteeringBehavior.Interpose        (this, agentA, agentB); }
			if (enableObstacleAvoidance) { steering_force += SteeringBehavior.ObstacleAvoidance(this, obstacles, minDetectionBoxLength); }
			if (enableOffsetPursuit)     { steering_force += SteeringBehavior.OffsetPursuit    (this, leader, offset); }
			if (enablePursuit)           { steering_force += SteeringBehavior.Pursuit          (this, evader); }
			if (enableSeparation)        { steering_force += SteeringBehavior.Separation       (this, nearbyVehicles); }
			if (enableWallAvoidance)     { steering_force += SteeringBehavior.WallAvoidance    (this, walls, wallFeelerLength); }
			if (enableWander)            { steering_force += SteeringBehavior.Wander           (this, wanderJitter, wanderRadius, wanderDistance, ref wanderTarget); }
			if (enableSeek)              { steering_force += SteeringBehavior.Seek             (this, seekTarget); }
			return steering_force;
		}
	}

	// Update is called once per frame
	public void update(Vector3 steering_force) {
		steering_force = Vector3.ClampMagnitude(steering_force, maxForce);
		var acceleration = steering_force / GetComponent<Rigidbody>().mass;
		velocity += acceleration * Time.deltaTime;
		velocity  = Vector3.ClampMagnitude(velocity, maxSpeed);
	}

	void Update() {
		if (enableTesting) {
			update(CalculateForces);
		}
		transform.position += velocity * Time.deltaTime;
		transform.rotation  = Quaternion.LookRotation(smoothRotation.update(velocity != Vector3.zero ? velocity : transform.forward));
		DebugExtension.DebugArrow(transform.position, velocity);
	}
}