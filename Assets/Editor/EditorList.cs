using UnityEngine;
using UnityEditor;

static public class EditorList {
	static public void Show(SerializedProperty list,
	                        EditorListOption options = EditorListOption.Default) {
		if (!list.isArray) {
			EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
			return;
		}
		bool showListLabel = (options & EditorListOption.ListLabel) != 0;
		bool showListSize  = (options & EditorListOption.ListSize)  != 0;
		if (showListLabel) {
			EditorGUILayout.PropertyField(list);
			EditorGUI.indentLevel += 1;
		}
		if (!showListLabel || list.isExpanded) {
			SerializedProperty size = list.FindPropertyRelative("Array.size");
			if (showListSize) {
				EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			}
			if (size.hasMultipleDifferentValues) {
				EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
			} else {
				ShowElements(list, options);
			}
		}
		if (showListLabel) {
			EditorGUI.indentLevel -= 1;
		}
	}

	static GUIContent moveButtonContent      = new GUIContent("↴", "move down");
	static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
	static GUIContent deleteButtonContent    = new GUIContent("-", "delete");
	static GUIContent addButtonContent       = new GUIContent("+", "add element");

	static void ShowElements(SerializedProperty list, EditorListOption options) {
		bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
		bool showButtons       = (options & EditorListOption.Buttons)       != 0;
		for (int i = 0; i < list.arraySize; ++i) {
			if (showButtons) {
				EditorGUILayout.BeginHorizontal();
			}
			if (showElementLabels) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
			} else {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
			}
			if (showButtons) {
				ShowButtons(list, i);
				EditorGUILayout.EndHorizontal();
			}
		}
		if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
			list.arraySize += 1;
		}
	}

	static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

	static void ShowButtons(SerializedProperty list, int index) {
		if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth)) {
			list.MoveArrayElement(index, index + 1);
		}
		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth)) {
			list.InsertArrayElementAtIndex(index);
		}
		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
			int oldSize = list.arraySize;
			list.DeleteArrayElementAtIndex(index);
			if (list.arraySize == oldSize) {
				list.DeleteArrayElementAtIndex(index);
			}
		}
	}
}