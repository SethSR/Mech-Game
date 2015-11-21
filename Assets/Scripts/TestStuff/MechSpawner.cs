using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections;

public class MechSpawner : MonoBehaviour {
	public Mech prefab;
	[iMin( 1)] public int   numberOfTeams;
	[iMin( 1)] public int   sizeOfTeams;
	[fMin(10)] public float spawnRadius;

	void Start() {
		for (int team_index = numberOfTeams; team_index --> 0;) {
			for (int mech_index = sizeOfTeams; mech_index --> 0;) {
				var position = Random.insideUnitSphere * spawnRadius;
				Mech mech = (Mech)Instantiate(prefab, position, Quaternion.identity);
				mech.team = team_index;
				mech.transform.parent = transform;
				mech.tag = "Mech";
			}
		}
	}

	void Update() {
		DebugExtension.DebugWireSphere(transform.position, Color.gray, spawnRadius);
	}
}