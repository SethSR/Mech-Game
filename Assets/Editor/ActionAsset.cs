using UnityEngine;
using UnityEditor;

public class ActionAsset {
	[MenuItem("Assets/Create/Action")]
	public static void CreateAsset() {
		ScriptableObjectUtility.CreateAsset<Actions>();
	}
}