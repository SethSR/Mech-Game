using UnityEngine;
using UnityEditor;
using System;

[Flags]
public enum EditorListOption {
	None            = 0,
	ListSize        = 1,
	ListLabel       = 2,
	ElementLabels   = 4,
	Buttons         = 8,
	NoElementLabels = ListSize | ListLabel,
	Default         = ListSize | ListLabel | ElementLabels,
	All             = Buttons  | Default,
}