using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections;

public class ResourceSpawner : MonoBehaviour {
	public Resource prefab;
	[iMin( 1)] public int numberOfResourceNodes;
	[fMin(10)] public float spawnRadius;
	[fMin( 1)] public float resourceAmountMin;
	[fMin( 1)] public float resourceAmountMax;

	void Start() {
		for (int i = numberOfResourceNodes; i --> 0;) {
			var position = Random.insideUnitSphere * spawnRadius;
			Resource res = (Resource)Instantiate(prefab, position, Quaternion.identity);
			res.startingAmount = resourceAmountMin + Random.value * (resourceAmountMax - resourceAmountMin);
			res.currentAmount = res.startingAmount;
			res.transform.parent = transform;
			res.tag = "Resource";
		}
	}
	
	void Update() {
		DebugExtension.DebugWireSphere(transform.position, Color.gray, spawnRadius);
	}
}