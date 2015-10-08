using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Reasoner))]
public class ReasonerEditor : Editor {
	public override void OnInspectorGUI() {
		serializedObject.Update();
		SerializedProperty actionDeciders = serializedObject.FindProperty("actionDeciders");
		// EditorList.Show(actionDeciders, EditorListOption.Buttons);
		EditorGUILayout.PropertyField(actionDeciders, true);
		serializedObject.ApplyModifiedProperties();
	}
}