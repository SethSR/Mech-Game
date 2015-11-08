using Vexe.Runtime.Types;
using UnityEngine;
using System.Collections.Generic;

public class GameController : BetterBehaviour {
	public Mech  prefab;
	public float spawnAreaSize;

	[iMin(2)] public int numberOfTeams;
	[iMin(1)] public int sizeOfTeams;

	public List<List<Mech>> teamList   = new List<List<Mech>>();
	public List<Color>      teamColors = new List<Color>();

	void Update() {
		DebugExtension.DebugWireSphere(transform.position, spawnAreaSize);
		if (teamList.Count < numberOfTeams) {
			teamList.Add(new List<Mech>(sizeOfTeams));
			teamColors.Add(new Color(Random.value, Random.value, Random.value, 1));
		}

		for (int i = teamList.Count; i --> 0;) {
			var team = teamList[i];
			if (team.Count < sizeOfTeams) {
				var  spawn_pos = Random.insideUnitSphere * spawnAreaSize;
				Mech mech = (Mech)Instantiate(prefab, spawn_pos, Quaternion.LookRotation(transform.position - spawn_pos));
				mech.team = i;
				mech.GetComponent<MeshRenderer>().materials[0].color = teamColors[i];
				team.Add(mech);
			}
		}
	}
}