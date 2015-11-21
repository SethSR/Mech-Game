using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections;

public class ObstacleSpawner : BetterBehaviour {
	[iMin( 1)] public int   numberOfObstacles;
	[fMin(10)] public float spawnRadius;
	[fMin( 1)] public float obstacleSizeMin;
	[fMin( 1)] public float obstacleSizeMax;

	void Start() {
		for (int i = numberOfObstacles; i --> 0;) {
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			go.transform.position = Random.insideUnitSphere * spawnRadius;
			go.transform.localScale = Vector3.one * (obstacleSizeMin + Random.value * (obstacleSizeMax - obstacleSizeMin));
			go.transform.parent = transform;
			go.tag = "Obstacle";
		}
	}

	void Update() {
		DebugExtension.DebugWireSphere(transform.position, Color.gray, spawnRadius);
	}
}