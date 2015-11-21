using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PropertyHolder)), CanEditMultipleObjects]
public class PropertyHolderEditor : Editor {
	public SerializedProperty propState;
	public SerializedProperty propValForAB;
	public SerializedProperty propValForA;
	public SerializedProperty propValForC;
	public SerializedProperty propControllable;

	void OnEnable() {
		// Setup the SerializedProperties
		propState        = serializedObject.FindProperty("state");
		propValForAB     = serializedObject.FindProperty("valForAB");
		propValForA      = serializedObject.FindProperty("valForA");
		propValForC      = serializedObject.FindProperty("valForC");
		propControllable = serializedObject.FindProperty("controllable");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUILayout.PropertyField(propState);

		PropertyHolder.Status st = (PropertyHolder.Status)propState.intValue;

		switch (st) {
			case PropertyHolder.Status.A:
				EditorGUILayout.PropertyField(propControllable, new GUIContent("controllable"));
				EditorGUILayout.IntSlider(propValForA, 0, 10, new GUIContent("valForA"));
				EditorGUILayout.IntSlider(propValForAB, 0, 100, new GUIContent("valForAB"));
				break;
			case PropertyHolder.Status.B:
				EditorGUILayout.PropertyField(propControllable, new GUIContent("controllable"));
				EditorGUILayout.IntSlider(propValForAB, 0, 100, new GUIContent("valForAB"));
				break;
			case PropertyHolder.Status.C:
				EditorGUILayout.PropertyField(propControllable, new GUIContent("controllable"));
				EditorGUILayout.IntSlider(propValForC, 0, 100, new GUIContent("valForC"));
				break;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}