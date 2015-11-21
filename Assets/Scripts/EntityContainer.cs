using UnityEngine;
using System.Collections.Generic;

public class EntityContainer : MonoBehaviour {
	public List<Mech>      mechs;
	public List<Resource>  resources;
	public List<Transform> obstacles;

	void Awake() {
		var mech_transform = transform.Find("Mechs");
		if (mech_transform != null) {
			foreach (Mech mech in mech_transform) {
				mechs.Add(mech);
			}
		}
		var res_transform = transform.Find("Resources");
		if (res_transform != null) {
			foreach (Resource res in res_transform) {
				resources.Add(res);
			}
		}
		var obs_transform = transform.Find("Obstacles");
		if (obs_transform != null) {
			foreach (Transform obs in obs_transform) {
				obstacles.Add(obs);
			}
		}
	}
}